using AnimalLibrary.Models;
using Azure;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SavannaWebApplication.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace SavannaWebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<GridCellModel> Grid { get; set; }
        public GameService GameService { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var httpClient = _httpClientFactory.CreateClient("Grid"); // Create HttpClient
            var httpResponseGrid = await httpClient.GetAsync("api/GridModel/GetGrid"); // Make GET request to the API
            var httpResponseGameService = await httpClient.GetAsync("api/GridModel/GetGameService"); // Make GET request to the API

            if (httpResponseGrid.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var jsonContent = await httpResponseGrid.Content.ReadAsStringAsync();

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
            if (httpResponseGameService.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var jsonContent = await httpResponseGameService.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignore case when mapping JSON to properties
                };

                var gameServiceDTO = JsonSerializer.Deserialize<GameServiceDTO> (jsonContent, options);


                if (gameServiceDTO != null)
                {
                    //GameService = gameServiceDTO;
                }
            }
        }
    }
}