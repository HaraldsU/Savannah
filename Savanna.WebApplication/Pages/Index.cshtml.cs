using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;
using SavannaWebApplication.Constants;
using SavannaWebApplication.Models;
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

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("Grid");
        }

        public async Task OnGet()
        {
            await OnGetSetSessionIdAsync();
            await OnGetAnimalListAsync();
        }
        public async Task<IActionResult> OnPostAddAnimalAsync(string animalName)
        {
            var gameId = HttpContext.Session.GetInt32(SessionConstants.GameId);
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = gameId,
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
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
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId)
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
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId)
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
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId)
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
        public async Task<IActionResult> OnPostStartGameAsync(string dimensions)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                SessionId = HttpContext.Session.GetInt32(SessionConstants.SessionId),
                Dimensions = dimensions
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/StartGame", requestContent);
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
        private async Task OnGetSetSessionIdAsync()
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
        private List<char> FormatGridForDisplay(List<GridCellModelDTO> grid)
        {
            List<char> formattedGrid = new();

            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    formattedGrid.Add(cell.Animal.FirstLetter);
                }
                else
                {
                    formattedGrid.Add(GridConstants.EmptyCell);
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