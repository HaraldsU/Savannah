using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;
using System.Text;

namespace Savanna.Cons.Handlers
{
    public class AnimalActionHandler
    {
        private readonly HttpClient _httpClient;
        private readonly Display _display;

        public AnimalActionHandler(HttpClient httpClient, Display display)
        {
            _httpClient = httpClient;
            _display = display;
        }

        public async Task<List<GridCellModelDTO>> OnPostAddAnimalAsync(string animalName)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                AnimalName = animalName,
                GameId = 0
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/AddAnimal", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var grid = JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson);

                return grid;
            }
            else
            {
                _display.DisplayErrorRetryMessage(ErrorMessageConstants.AddAnimalFailed, response.StatusCode);
                return new List<GridCellModelDTO>();
            }
        }
        public async Task<List<GridCellModelDTO>> OnPostMoveAnimalsAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = 0
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/MoveAnimals", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var grid = JsonConvert.DeserializeObject<List<GridCellModelDTO>>(responseJson);

                return grid;
            }
            else
            {
                _display.DisplayErrorRetryMessage(ErrorMessageConstants.MoveAnimalsFailed, response.StatusCode);
                return new List<GridCellModelDTO>();
            }
        }
    }
}
