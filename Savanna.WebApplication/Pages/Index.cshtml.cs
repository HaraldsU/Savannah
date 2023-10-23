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
        public string Grid;

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
            await GetAnimalListAsync();
            await GetInitializedGridAsync();
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = HttpContext.Session.GetInt32(SessionConstants.GameId)
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/MoveAnimals", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var grid = FormatGridForDisplay(JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson));
                UpdateGameInfo(responseJson);

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
        public async Task<IActionResult> OnPostAddAnimalAsync(string animalName)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = HttpContext.Session.GetInt32(SessionConstants.GameId),
                AnimalName = animalName
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/AddAnimal", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var grid = FormatGridForDisplay(JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson));
                UpdateGameInfo(responseJson);

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
        public async Task<IActionResult> OnPostSaveGameAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = HttpContext.Session.GetInt32(SessionConstants.GameId)
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
        public async Task<IActionResult> OnPostLoadGameAsync(string gameId)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = gameId
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
        private async Task GetInitializedGridAsync()
        {
            var gameId = HttpContext.Session.GetInt32(SessionConstants.GameId);
            if (gameId == null)
            {
                var response = await _httpClient.GetAsync("api/Game/GetInitializedGrid");
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ResponseModel>(responseJson);

                    gameId = responseData.GameId;
                    Grid = JsonConvert.SerializeObject(responseData.Grid);

                    HttpContext.Session.SetInt32(SessionConstants.GameId, (int)gameId);
                    HttpContext.Session.SetInt32(SessionConstants.GridDimensions, responseData.Grid.Count);
                    UpdateGameInfo(Grid);
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveGridFailed}: {response.StatusCode}");
                }
            }
        }
        private async Task GetAnimalListAsync()
        {
            var animalList = HttpContext.Session.GetString(SessionConstants.AnimalList);
            if (animalList == null)
            {
                var response = await _httpClient.GetAsync("api/Game/GetAnimalPluginList");
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString(SessionConstants.AnimalList, responseJson);
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
                }
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
            var gridDTO = JsonConvert.DeserializeObject<List<GridCellModelDTO>>(grid);
            var animalsString = HttpContext.Session.GetString(SessionConstants.AnimalList);
            var animalsDTO = JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(animalsString);

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