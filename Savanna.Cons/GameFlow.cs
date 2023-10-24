using Savanna.Commons.Models;
using System.Net.Http;

namespace Savanna.Cons
{
    public class GameFlow
    {
        private int dimension;
        private List<AnimalBaseDTO>? animals;
        private readonly Display _display;
        private readonly Input _input;
        private ApiRequests? _apiRequests;
        public GameFlow()
        {
            _display = new();
            _input = new();
        }
        public async Task Run(HttpClient httpClient)
        {
            _apiRequests = new(httpClient);
            var validationErrorList = await _apiRequests.GetAnimalPluginListValidationsAsync();
            if (string.IsNullOrWhiteSpace(validationErrorList))
            {
                _display.DisplayPluginLoadValidationError(validationErrorList);
                Environment.Exit(0);
            }
            dimension = _input.GridSizeInput();
            animals = await _apiRequests.GetAnimalListAsync();

            _display.DisplayAnimalCount(animals);
            _display.DisplayGameTitle();

            int cursorTop = Console.CursorTop;
            bool isGameRunning = true;
            var grid = await _apiRequests.PostInitializedGridAsync(dimension);


            while (isGameRunning)
            {
                _display.DisplayGrid(grid, animals, cursorTop);
                grid = await _apiRequests.OnPostMoveAnimalsAsync(); 
                Thread.Sleep(500);

                var buttonListener = await _input.ButtonListener(_apiRequests, animals);
                if (buttonListener != null)
                {
                    grid = buttonListener;
                }
                _display.DisplayGameplayInfo(animals);
            }
        }
    }
}
