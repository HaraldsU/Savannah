using Newtonsoft.Json;
using Savanna.Commons.Constants;
using Savanna.Commons.Models;

namespace Savanna.Cons.Handlers
{
    public class AnimalListHandler
    {
        private readonly HttpClient _httpClient;
        private readonly Display _display;

        public AnimalListHandler(HttpClient httpClient, Display display)
        {
            _httpClient = httpClient;
            _display = display;
        }

        public async Task<List<AnimalBaseDTO>> OnGetAnimalListAsync()
        {
            int maxTries = RetryPolicyConstants.maxTries;
            do
            {
                var response = await _httpClient.GetAsync("api/Game/AnimalList");
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<AnimalBaseDTO>>(responseJson);
                }
                else
                {
                    maxTries = await _display.DisplayErrorRetryMessage(ErrorMessageConstants.RetrieveAnimalListFailed, response.StatusCode, maxTries, RetryPolicyConstants.timeOut);
                }
            } while (maxTries > 0);

            throw new Exception(ErrorMessageConstants.MaxRetriesReached);
        }
        public async Task<string> OnGetAnimalValidationErrorsAsync()
        {
            int maxTries = RetryPolicyConstants.maxTries;
            do
            {
                var response = await _httpClient.GetAsync("api/Game/AnimalValidationErrors");
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return responseJson;
                }
                else
                {
                    maxTries = await _display.DisplayErrorRetryMessage(ErrorMessageConstants.RetrieveAnimalListFailed, response.StatusCode, maxTries, RetryPolicyConstants.timeOut);
                }
            } while (maxTries > 0);

            throw new Exception(ErrorMessageConstants.MaxRetriesReached);
        }
    }
}
