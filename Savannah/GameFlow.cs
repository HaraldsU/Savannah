using ClassLibrary;
using ClassLibrary.Services;
using Savanna.cons;

namespace Savannah
{
    public class GameFlow
    {
        public int Dimension;
        private readonly GridService _initializeGrid;
        private readonly Display _display;
        private readonly Input _input;
        private readonly ButtonListenerService? _buttonListener;
        private readonly GameService? _gameService;
        public GameFlow()
        {
            _initializeGrid = new();
            _display = new();
            _input = new();
            _buttonListener = new();
            _gameService = new();
        }
        public void Run()
        {
            var validationErrorList = AnimalListSingleton.Instance.GetAnimalListValidationErrors();
            if (validationErrorList != String.Empty)
            {
                _display.DisplayPluginLoadValidationError(validationErrorList);
                Environment.Exit(0);
            }
            Dimension = _input.GridSizeInput();

            _display.DisplayGameTitle();

            int cursorTop = Console.CursorTop;
            bool isGameRunning = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (isGameRunning)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                _gameService.MoveAnimals(Dimension, grid);
                Thread.Sleep(250);

                _buttonListener.ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
    }
}
