using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using NuGet.ProjectModel;
using NuGet.Protocol;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;
using SavannaWebApplication.Constants;
using SavannaWebApplication.Models;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> GameInfo = new();
        public string? Grid;

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _config;

        private readonly string _baseUri = "https://api.bing.microsoft.com/v7.0/images/search";

        private const string QUERY_PARAMETER = "?q=";  // Required
        private const string MKT_PARAMETER = "&mkt=";  // Strongly suggested

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Grid");
            _config = config;
        }

        public async Task OnGet()
        {
            await SetSessionIdAsync();
            await SetCurrentUserIdSession();
            await OnGetAnimalListAsync();
            await OnPostAnimalImagesAsync();
        }
        public async Task<IActionResult> OnPostAddAnimalAsync(string animalName)
        {
            var gameId = HttpContext.Session.GetInt32(SessionConstants.GameId);
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = gameId,
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                UserId = HttpContext.Session.GetString(SessionConstants.UserId),
                AnimalName = animalName
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/AddAnimal", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                Grid = responseJson;
                UpdateGameInfo(Grid);

                var grid = FormatGridForDisplay(JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson));
                var returnData = new
                {
                    grid,
                    GameInfo
                };

                return new JsonResult(returnData);
            }
            else
            {
                return new JsonResult(new { success = false, message = ErrorMessageConstants.AddAnimalFailed })
                {
                    StatusCode = 400
                };
            }
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = HttpContext.Session.GetInt32(SessionConstants.GameId),
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                UserId = HttpContext.Session.GetString(SessionConstants.UserId)
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/MoveAnimals", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                Grid = responseJson;
                UpdateGameInfo(Grid);

                var grid = FormatGridForDisplay(JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson));
                var returnData = new
                {
                    Grid = grid,
                    GameInfo
                };

                return new JsonResult(returnData);
            }
            else
            {
                return new JsonResult(new { success = false, message = ErrorMessageConstants.MoveAnimalsFailed })
                {
                    StatusCode = 400
                };
            }
        }
        public async Task<IActionResult> OnPostLoadGameAsync(string gameId)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = gameId,
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                UserId = HttpContext.Session.GetString(SessionConstants.UserId)
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/LoadGame", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var isGame = JsonConvert.DeserializeObject<bool>(responseJson);

                if (isGame)
                {
                    HttpContext.Session.SetInt32(SessionConstants.GameId, int.Parse(gameId));
                }

                return new JsonResult(responseJson);
            }
            else
            {
                return new JsonResult(new { success = false, message = ErrorMessageConstants.SaveGameFailed })
                {
                    StatusCode = 400
                };
            }
        }
        public async Task<IActionResult> OnPostSaveGameAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = HttpContext.Session.GetInt32(SessionConstants.GameId),
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                UserId = HttpContext.Session.GetString(SessionConstants.UserId)
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/SaveGame", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return new JsonResult(responseJson);
            }
            else
            {
                return new JsonResult(new { success = false, message = ErrorMessageConstants.SaveGameFailed })
                {
                    StatusCode = 400
                };
            }
        }
        public async Task<IActionResult> OnPostNewGameAsync(string dimensions)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                UserId = HttpContext.Session.GetString(SessionConstants.UserId),
                Dimensions = dimensions
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/NewGame", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ResponseModel>(responseJson);

                var gameId = responseData.GameId;
                Grid = JsonConvert.SerializeObject(responseData.Grid);

                HttpContext.Session.SetInt32(SessionConstants.GameId, gameId);
                HttpContext.Session.SetInt32(SessionConstants.GridDimensions, (int)MathF.Sqrt(responseData.Grid.Count));
                UpdateGameInfo(Grid);

                return new JsonResult(HttpContext.Session.GetInt32(SessionConstants.GridDimensions));
            }
            return new JsonResult(new { success = false, message = ErrorMessageConstants.StartGameFailed })
            {
                StatusCode = 400
            };
        }

        private async Task OnGetAnimalListAsync()
        {
            var animalList = HttpContext.Session.GetString(SessionConstants.AnimalList);
            if (animalList == null)
            {
                var response = await _httpClient.GetAsync("api/Game/AnimalList");
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString(SessionConstants.AnimalList, responseJson);
                    HttpContext.Session.SetInt32(SessionConstants.GridDimensions, 0);

                    UpdateGameInfo(Grid);
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
                }
            }
        }
        private async Task OnPostAnimalImagesAsync()
        {
            bool successOuter;
            do
            {
                successOuter = true;
                try
                {
                    var animalImageList = new List<Tuple<string, string>>();
                    var animalList = HttpContext.Session.GetString(SessionConstants.AnimalList);
                    var animalListDTO = JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(animalList);

                    foreach (var animal in animalListDTO)
                    {
                        var searchString = $"{animal.Name} animal pixelated icon transparent";

                        // Remember to encode the q query parameter.
                        var queryString = QUERY_PARAMETER + Uri.EscapeDataString(searchString);
                        queryString += MKT_PARAMETER + "en-us";

                        HttpResponseMessage response = await AnimalImages(queryString);

                        var contentString = await response.Content.ReadAsStringAsync();
                        Dictionary<string, object> searchResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);

                        if (response.IsSuccessStatusCode)
                        {
                            var imageIndex = 0;
                            var animalImage = searchResponse["value"].ToJToken()[imageIndex]["contentUrl"];
                            var animalImageValue = ((Newtonsoft.Json.Linq.JValue)animalImage).Value;

                            HttpClient client = new();
                            HttpResponseMessage imageResponse;

                            bool successInner;
                            do
                            {
                                successInner = true;
                                try
                                {
                                    imageResponse = await client.GetAsync(animalImageValue.ToString());
                                }
                                catch (Exception e)
                                {
                                    imageIndex++;
                                    animalImage = searchResponse["value"].ToJToken()[imageIndex]["contentUrl"];
                                    animalImageValue = ((Newtonsoft.Json.Linq.JValue)animalImage).Value;

                                    successInner = false;
                                    Debug.WriteLine(e.Message);
                                }
                                
                            } while (!successInner);

                            animalImageList.Add(Tuple.Create(animal.Name, animalImageValue.ToString()));
                        }
                        else
                        {
                            PrintErrors(response.Headers, searchResponse);
                        }

                    }
                    HttpContext.Session.SetString(SessionConstants.AnimalImages, JsonConvert.SerializeObject(animalImageList));
                }
                catch (Exception e)
                {
                    successOuter = false;
                    Debug.WriteLine(e.Message);
                }
            } while (!successOuter);
            var animalImageListNew = JsonConvert.DeserializeObject<List<Tuple<string, string>>>(HttpContext.Session.GetString(SessionConstants.AnimalImages));
        }
        static void PrintErrors(HttpResponseHeaders headers, Dictionary<String, object> response)
        {
            Debug.WriteLine("The response contains the following errors:\n");

            object value;

            if (response.TryGetValue("error", out value))  // typically 401, 403
            {
                PrintError(response["error"] as Newtonsoft.Json.Linq.JToken);
            }
            else if (response.TryGetValue("errors", out value))
            {
                // Bing API error

                foreach (Newtonsoft.Json.Linq.JToken error in response["errors"] as Newtonsoft.Json.Linq.JToken)
                {
                    PrintError(error);
                }

                // Included only when HTTP status code is 400; not included with 401 or 403.

                IEnumerable<string> headerValues;
                if (headers.TryGetValues("BingAPIs-TraceId", out headerValues))
                {
                    Debug.WriteLine("\nTrace ID: " + headerValues.FirstOrDefault());
                }
            }

        }
        static void PrintError(Newtonsoft.Json.Linq.JToken error)
        {
            string value = null;

            Debug.WriteLine("Code: " + error["code"]);
            Debug.WriteLine("Message: " + error["message"]);

            if ((value = (string)error["parameter"]) != null)
            {
                Debug.WriteLine("Parameter: " + value);
            }

            if ((value = (string)error["value"]) != null)
            {
                Debug.WriteLine("Value: " + value);
            }
        }
        async Task<HttpResponseMessage> AnimalImages(string queryString)
        {
            var client = new HttpClient();

            // Request headers. The subscription key is the only required header but you should
            // include User-Agent (especially for mobile), X-MSEdge-ClientID, X-Search-Location
            // and X-MSEdge-ClientIP (especially for local aware queries).

            var subscriptionKey = _config["BingSearchApi:SubscriptionKey"];

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            return (await client.GetAsync(_baseUri + queryString));
        }

        private async Task SetSessionIdAsync()
        {
            var response = await _httpClient.GetAsync("api/Game/SessionId");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetInt32(SessionConstants.SessionId, int.Parse(responseJson));
            }
            else
            {
                throw new Exception($"{ErrorMessageConstants.RetrieveSessionIdFailed}: {response.StatusCode}");
            }
        }
        private async Task SetCurrentUserIdSession()
        {
            HttpContext.Session.SetString(SessionConstants.UserId, User.Identity.Name);
        }
        private List<string> FormatGridForDisplay(List<GridCellModelDTO> grid)
        {
            List<string> formattedGrid = new();
            var animalImageList = JsonConvert.DeserializeObject<List<Tuple<string, string>>>(HttpContext.Session.GetString(SessionConstants.AnimalImages));

            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    var animalImage = animalImageList.Where(x => x.Item1 == cell.Animal.Name).FirstOrDefault().Item2;
                    //formattedGrid.Add(cell.Animal.FirstLetter);
                    formattedGrid.Add(animalImage);
                }
                else
                {
                    //formattedGrid.Add(GridConstants.EmptyCell);
                    var emptyCell = GridConstants.EmptyCell.ToString();
                    formattedGrid.Add(emptyCell);
                }
            }

            return formattedGrid;
        }
        private void UpdateGameInfo(string grid)
        {
            Dictionary<string, int> animalCount = new();
            var animalsString = HttpContext.Session.GetString(SessionConstants.AnimalList);
            var animalsDTO = JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(animalsString);

            var gridDTO = new List<GridCellModelDTO>();
            if (Grid != null)
            {
                gridDTO = JsonConvert.DeserializeObject<List<GridCellModelDTO>>(grid);
            }

            GameInfo.Add($"Grid size: {gridDTO.Count}");
            foreach (var animal in animalsDTO)
            {
                if (animal.Name != null)
                {
                    animalCount.Add(animal.Name, 0);
                }
                for (int i = 0; i < gridDTO.Count; i++)
                {
                    if (gridDTO[i].Animal != null)
                    {
                        if (gridDTO[i].Animal.Name == animal.Name)
                        {
                            animalCount[animal.Name]++;
                        }
                    }
                }
            }

            foreach (var animal in animalCount)
            {
                GameInfo.Add($"{animal.Key}: {animal.Value}");
            }
        }
    }
}