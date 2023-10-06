using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SavannaWebApplication.Models;
using System.Text;
using System.Text.Json;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        public static GridModelDTO Grid { get; set; }
        public List<PluginBaseDTO> Animals { get; set; } = new List<PluginBaseDTO>();
        private static bool GridDataRetrieved = false;
        private static bool IsPredatorTurn = true;
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
            await InitializeGrid();
            await GetAnimalPluginList();
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var url = _httpClient.BaseAddress.AbsoluteUri + "api/Game/MoveAnimals";
            var isPredatorTurn = IsPredatorTurn == true ? "true" : "false";
            var jsonData = JsonConvert.SerializeObject(new
            {
                Grid,
                isPredatorTurn
            });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (IsPredatorTurn == true)
            {
                IsPredatorTurn = false;
            }
            else
            {
                IsPredatorTurn = true;
            }

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                GridModelDTO gridData = JsonConvert.DeserializeObject<GridModelDTO>(responseData);
                Grid = gridData;
                return new JsonResult(Grid.Grid);
            }
            else
            {
                var errorMessage = "Move Animals Failed !!!";
                return new JsonResult(new { success = false, message = errorMessage })
                {
                    StatusCode = 400
                };
            }
        }
        public async Task<IActionResult> OnPostAddAnimalAsync(string animalName)
        {
            var url = _httpClient.BaseAddress.AbsoluteUri + "api/Game/AddAnimal";
            var jsonData = JsonConvert.SerializeObject(new
            {
                animalName,
                Grid
            });
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                GridModelDTO gridData = JsonConvert.DeserializeObject<GridModelDTO>(responseData);
                Grid = gridData;
                return new JsonResult(Grid.Grid);
            }
            else
            {
                var errorMessage = "Add Animals Failed !!!";
                return new JsonResult(new { success = false, message = errorMessage })
                {
                    StatusCode = 400
                };
            }
        }
        private async Task InitializeGrid()
        {
            if (!GridDataRetrieved)
            {
                var response = await _httpClient.GetAsync("api/Game/GetGrid");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var gridModelDTO = System.Text.Json.JsonSerializer.Deserialize<GridModelDTO>(responseData, options);
                    if (gridModelDTO != null)
                    {
                        Grid = gridModelDTO;
                        GridDataRetrieved = true;
                    }
                }
            }
        }
        private async Task GetAnimalPluginList()
        {
            var response = await _httpClient.GetAsync("api/Game/GetGameService"); // Make GET request to the API
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
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
                        IsPrey = dto.IsPrey,
                        Color = dto.Color,
                        Speed = dto.Speed,
                        Range = dto.Range,
                        Health = dto.Health,
                        BreedingCooldown = dto.BreedingCooldown,
                        BreedingTime = dto.BreedingTime,
                        ActiveBreedingCooldown = dto.ActiveBreedingCooldown,
                        IsBirthing = dto.IsBirthing
                    };
                    Animals.Add(animal);
                }
            }
        }
    }
}