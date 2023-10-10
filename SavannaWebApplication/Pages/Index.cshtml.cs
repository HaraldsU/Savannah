using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SavannaWebApplication.Constants;
using SavannaWebApplication.Models;
using System.Text;
using System.Text.Json;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static GridModelDTO? Grid { get; set; } = null;
        public static List<PluginBaseDTO> Animals { get; set; } = new();
        public List<string> GameInfo { get; set; } = new();
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
            await GetGridAsync();
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var jsonData = JsonConvert.SerializeObject(new
            {
                Grid,
            });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Game/MoveAnimals", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                Grid = JsonConvert.DeserializeObject<GridModelDTO>(responseData);
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
                Grid
            });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Game/AddAnimal", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                Grid = JsonConvert.DeserializeObject<GridModelDTO>(responseData);
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
        private List<char> FormatGridForDisplay()
        {
            List<char> formattedGrid = new();
            foreach (var cell in Grid.Grid)
            {
                if (cell.Animal != null)
                {
                    if (cell.Animal.Predator != null)
                    {
                        formattedGrid.Add(cell.Animal.Predator.FirstLetter);
                    }
                    else if (cell.Animal.Prey!= null)
                    {
                        formattedGrid.Add(cell.Animal.Prey.FirstLetter);
                    }
                    else
                    {
                        formattedGrid.Add(GridConstants.EmptyCell);
                    }
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
            GameInfo.Add($"Grid size: {Grid.Grid.Count}");
            foreach (var animal in Animals)
            {
                animalCount.Add(animal.Name, 0);
                for (int i = 0; i < Grid.Grid.Count; i++)
                {
                    if (Grid.Grid[i].Animal != null)
                    {
                        if (Grid.Grid[i].Animal.Predator != null)
                        {
                            if (Grid.Grid[i].Animal.Predator.Name == animal.Name)
                            {
                                animalCount[animal.Name]++;
                            }
                        }
                        else if (Grid.Grid[i].Animal.Prey != null)
                        {
                            if (Grid.Grid[i].Animal.Prey.Name == animal.Name)
                            {
                                animalCount[animal.Name]++;
                            }
                        }
                    }
                }
            }
            foreach (var animal in animalCount)
            {
                GameInfo.Add($"{animal.Key}: {animal.Value}");
            }
        }
        private async Task GetGridAsync()
        {
            if (Grid == null)
            {
                var response = await _httpClient.GetAsync("api/Game/GetGrid");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Grid = System.Text.Json.JsonSerializer.Deserialize<GridModelDTO>(responseData, options);
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
            if (Animals.Count == 0)
            {
                var response = await _httpClient.GetAsync("api/Game/GetAnimalPluginList"); // Make GET request to the API
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var pluginBaseDTOList = System.Text.Json.JsonSerializer.Deserialize<List<PluginBaseDTO>>(responseData, options);
                    foreach (var dto in pluginBaseDTOList)
                    {
                        var animal = new PluginBaseDTO
                        {
                            Name = dto.Name,
                            FirstLetter = dto.FirstLetter,
                            KeyBind = dto.KeyBind,
                            Color = dto.Color,
                        };
                        Animals.Add(animal);
                    }
                }
                else
                {
                    throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
                }
            }
        }
    }
}