using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;
using Savanna.Cons.Models;
using System.Text;

namespace Savanna.Cons.Handlers
{
    public class GameHandler
    {
        private readonly HttpClient _httpClient;
        private readonly Display _display;

        public GameHandler(HttpClient httpClient, Display display)
        {
            _httpClient = httpClient;
            _display = display;
        }

        public async Task<List<GridCellModelDTO>> OnPostStartGameAsync(int dimensions)
        {
            int maxTries = RetryPolicyConstants.maxTries;
            do
            {
                var requestData = JsonConvert.SerializeObject(new
                {
                    SessionId = 0,
                    Dimensions = dimensions
                });
                var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Game/StartGame", requestContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ResponseModel>(responseJson);

                    var grid = responseData.Grid;

                    return grid;
                }
                else
                {
                    maxTries = await _display.DisplayErrorRetryMessage(ErrorMessageConstants.StartGameFailed, response.StatusCode, maxTries, RetryPolicyConstants.timeOut);
                }
            } while (maxTries > 0);

            throw new Exception(ErrorMessageConstants.MaxRetriesReached);
        }
        public async Task<List<GridCellModelDTO>> OnPostAddAnimalAsync(string animalName)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId = 1,
                SessionId = 0,
                AnimalName = animalName
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
                GameId = 1,
                SessionId = 0
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
