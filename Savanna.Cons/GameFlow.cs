using Savanna.Commons.Models;
using Savanna.Cons.Facade;

namespace Savanna.Cons
{
    public class GameFlow
    {
        private int dimension;
        private List<AnimalBaseDTO>? animals;
        private readonly Display _display;
        private readonly Input _input;
        private GameFacade? _gameFacade;

        public GameFlow()
        {
            _display = new();
            _input = new();
        }

        public async Task RunGame(HttpClient httpClient)
        {
            _gameFacade = new(httpClient, _display);

            var validationErrorList = await _gameFacade.GetAnimalValidationErrorsAsync();
            if (string.IsNullOrWhiteSpace(validationErrorList))
            {
                _display.DisplayPluginLoadValidationError(validationErrorList);
                Environment.Exit(0);
            }

            dimension = _input.GridSizeInput();
            animals = await _gameFacade.GetAnimalListAsync();

            _display.DisplayAnimalCount(animals);

            bool isGameRunning = true;
            var grid = await _gameFacade.StartGameAsync(dimension);

            while (isGameRunning)
            {
                _display.DisplayGame(grid, animals);

                grid = await _gameFacade.MoveAnimalsAsync();
                Thread.Sleep(500);

                var buttonListener = await _input.ButtonListener(_gameFacade, animals);
                if (buttonListener.Count != 0)
                {
                    grid = buttonListener;
                }
            }
        }
    }
}
