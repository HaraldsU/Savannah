using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;
using Savanna.Cons.Models;
using System.Text;

namespace Savanna.Cons
{
    public class ApiRequests
    {
        private readonly HttpClient _httpClient;
        private static int GameId;
        public ApiRequests(HttpClient httpClient)
        { 
            _httpClient = httpClient;
        }

        public async Task<string> OnGetAnimalPluginListValidationsAsync()
        {
            var response = await _httpClient.GetAsync("api/Game/GetAnimalValidationErrors");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return responseJson;
            }
            else
            {
                throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
            }
        }
        public async Task<List<AnimalBaseDTO>> OnGetAnimalListAsync()
        {
            var response = await _httpClient.GetAsync("api/Game/GetAnimalPluginList");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(responseJson);
            }
            else
            {
                throw new Exception($"{ErrorMessageConstants.RetrieveAnimalListFailed}: {response.StatusCode}");
            }
        }
        public async Task<List<GridCellModelDTO>> OnPostInitializedGridAsync(int dimensions)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                Dimensions = dimensions
            });
            var requestContent = new StringContent(requestData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Game/PostInitializedGrid", requestContent);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ResponseModel>(responseJson);

                var gameId = responseData.GameId;
                var grid = responseData.Grid;

                GameId = (int)gameId;

                return grid;
            }
            else
            {
                throw new Exception($"{ErrorMessageConstants.RetrieveGridFailed}: {response.StatusCode}");
            }
        }
        public async Task<List<GridCellModelDTO>> OnPostMoveAnimalsAsync()
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                GameId
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
                throw new Exception($"{ErrorMessageConstants.RetrieveGridFailed}: {response.StatusCode}");
            }
        }
        public async Task<List<GridCellModelDTO>> OnPostAddAnimalAsync(string animalName)
        {
            var requestData = JsonConvert.SerializeObject(new
            {
                AnimalName = animalName,
                GameId
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
                throw new Exception($"{ErrorMessageConstants.RetrieveGridFailed}: {response.StatusCode}");
            }
        }
    }
}
