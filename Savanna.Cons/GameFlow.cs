using Savanna.Services;

namespace Savanna.Cons
{
    public class GameFlow
    {
        public int Dimension;
        private readonly GridService _initializeGrid;
        private readonly Display _display;
        private readonly Input _input;
        private readonly GameService? _gameService;
        public GameFlow()
        {
            _initializeGrid = new();
            _display = new();
            _input = new();
            _gameService = new();
        }
        public void Run()
        {
            var validationErrorList = _gameService.ValidationErrors;
            if (validationErrorList != String.Empty)
            {
                _display.DisplayPluginLoadValidationError(validationErrorList);
                Environment.Exit(0);
            }
            Dimension = _input.GridSizeInput();

            _display.DisplayAnimalCount();
            _display.DisplayGameTitle();

            int cursorTop = Console.CursorTop;
            bool isGameRunning = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (isGameRunning)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                //_gameService.MoveAnimals(Dimension, grid);
                Thread.Sleep(500);

                _input.ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
    }
}
