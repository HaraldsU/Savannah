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
        private readonly ILogger<IndexModel> _logger;
        public static GridModelDTO? Grid { get; set; } = null;
        public List<PluginBaseDTO> Animals { get; set; } = new List<PluginBaseDTO>();
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
            await GetGrid();
            await GetAnimalPluginList();
        }
        public async Task<IActionResult> OnPostMoveAnimalsAsync()
        {
            var url = _httpClient.BaseAddress.AbsoluteUri + "api/Game/MoveAnimals";
            var jsonData = JsonConvert.SerializeObject(new
            {
                Grid,
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
        private async Task GetGrid()
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

                    var gridModelDTO = System.Text.Json.JsonSerializer.Deserialize<GridModelDTO>(responseData, options);
                    Grid = gridModelDTO;
                }
                else
                {

                }
            }
        }
        private async Task GetAnimalPluginList()
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
        }
    }
}