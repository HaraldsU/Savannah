using AnimalLibrary.Models;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SavannaWebApplication.Models;
using System.Text.Json;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<GridCellModel> Grid { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var httpClient = _httpClientFactory.CreateClient("Grid"); // Create HttpClient
            
            var httpResponse = await httpClient.GetAsync("api/GridModel"); // Make GET request to the API

            if (httpResponse.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var jsonContent = await httpResponse.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignore case when mapping JSON to properties
                };

                var gridModelDTO = JsonSerializer.Deserialize<GridModelDTO>(jsonContent, options);


                if (gridModelDTO != null)
                {
                    Grid = gridModelDTO.Grid;
                }
            }

        }
    }
}