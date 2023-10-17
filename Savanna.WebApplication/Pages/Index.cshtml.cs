using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Enums;
using SavannaWebApplication.Constants;
using SavannaWebApplication.Models;
using System.Text;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public List<string> GameInfo { get; set; } = new();
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
            await GetAnimalPluginListAsync();
            await GetInitializedGridAsync();
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var jsonData = JsonConvert.SerializeObject(new
            {
                Grid = HttpContext.Session.GetString(SessionConstants.Grid),
                Turn = HttpContext.Session.GetInt32(SessionConstants.GridTurn),
                CurrentTypeIndex = HttpContext.Session.GetInt32(SessionConstants.GridCurrentTypeIndex)
            });

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Game/MoveAnimals", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var request = JsonConvert.DeserializeObject<RequestsModel>(responseData);

                var grid = request.Grid;
                var turn = request.Turn;
                var currentTypeIndex = request.CurrentTypeIndex;

                var gridModelDTO = new GridModelDTO
                {
                    Grid = grid
                };

                string gridModelJson = JsonConvert.SerializeObject(gridModelDTO);

                HttpContext.Session.SetString(SessionConstants.Grid, gridModelJson);
                HttpContext.Session.SetInt32(SessionConstants.GridTurn, (int)turn);
                HttpContext.Session.SetInt32(SessionConstants.GridCurrentTypeIndex, (int)currentTypeIndex);

                var formattedGrid = FormatGridForDisplay();
                UpdateGameInfo();

                var resultData = new
                {
                    formattedGrid,
                    GameInfo
                };

                return new JsonResult(resultData);
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
            var jsonData = JsonConvert.SerializeObject(new
            {
                animalName,
                Grid = HttpContext.Session.GetString(SessionConstants.Grid)
            });

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Game/AddAnimal", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString(SessionConstants.Grid, responseData);
                var formattedGrid = FormatGridForDisplay();
                UpdateGameInfo();

                var resultData = new
                {
                    formattedGrid,
                    GameInfo
                };

                return new JsonResult(resultData);
            }
            else
            {
                return new JsonResult(new { success = false, message = ErrorMessageConstants.AddAnimalFailed })
                {
                    StatusCode = 400
                };
            }
        }
        private async Task GetInitializedGridAsync()
        {
            var grid = HttpContext.Session.GetString(SessionConstants.Grid);
            if (grid == null)
            {
                var response = await _httpClient.GetAsync("api/Game/GetGrid");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var gridTmp = JsonConvert.DeserializeObject<GridModelDTO>(responseData);

                    HttpContext.Session.SetString(SessionConstants.Grid, responseData);
                    HttpContext.Session.SetInt32(SessionConstants.GridDimensions, gridTmp.Grid.Count);
                    HttpContext.Session.SetInt32(SessionConstants.GridTurn, (int)AnimalTypeEnums.Predator);
                    HttpContext.Session.SetInt32(SessionConstants.GridCurrentTypeIndex, 0);

                    UpdateGameInfo();
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveGridFailed}: {response.StatusCode}");
                }
            }
        }
        private async Task GetAnimalPluginListAsync()
        {
            HttpContext.Session.SetString(SessionConstants.AnimalList, string.Empty);
            var animalList = HttpContext.Session.GetString(SessionConstants.AnimalList);
            if (animalList == string.Empty)
            {
                var response = await _httpClient.GetAsync("api/Game/GetAnimalPluginList");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var pluginBaseDTOList = JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(responseData);
                    List<AnimalBaseDTO> animals = new();

                    foreach (var dto in pluginBaseDTOList)
                    {
                        var animal = new AnimalBaseDTO
                        {
                            Name = dto.Name,
                            FirstLetter = dto.FirstLetter,
                            KeyBind = dto.KeyBind,
                            Color = dto.Color,
                        };
                        animals.Add(animal);
                    }

                    var animalsJson = JsonConvert.SerializeObject(animals);
                    HttpContext.Session.SetString(SessionConstants.AnimalList, animalsJson);
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
                }
            }
        }
        private List<char> FormatGridForDisplay()
        {
            List<char> formattedGrid = new();
            var grid = HttpContext.Session.GetString(SessionConstants.Grid);
            var gridDTO = JsonConvert.DeserializeObject<GridModelDTO>(grid);

            foreach (var cell in gridDTO.Grid)
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
        private void UpdateGameInfo()
        {
            Dictionary<string, int> animalCount = new();
            var gridString = HttpContext.Session.GetString(SessionConstants.Grid);
            var gridDTO = JsonConvert.DeserializeObject<GridModelDTO>(gridString);
            var animalsString = HttpContext.Session.GetString(SessionConstants.AnimalList);
            var animalsDTO = JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(animalsString);

            GameInfo.Add($"Grid size: {gridDTO.Grid.Count}");
            foreach (var animal in animalsDTO)
            {
                if (animal.Name != null)
                {
                    animalCount.Add(animal.Name, 0);
                }
                for (int i = 0; i < gridDTO.Grid.Count; i++)
                {
                    if (gridDTO.Grid[i].Animal != null)
                    {
                        if (gridDTO.Grid[i].Animal.Name == animal.Name)
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