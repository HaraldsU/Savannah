using ClassLibrary;
using ClassLibrary.PluginHandlers;
using Savanna.cons;

namespace Savannah
{
    public class GameFlow
    {
        public int Dimension;
        private readonly GridService _initializeGrid;
        private Input _input;
        private GameService _gameService;
        private Display _display;
        private readonly PluginLoader _pluginLoader;
        public GameFlow()
        {
            _initializeGrid = new();
            _pluginLoader = new();
        }
        public void Run()
        {
            var pluginList = _pluginLoader.LoadPlugins();
            if (pluginList.Item2 != String.Empty)
            {
                _display = new(pluginList.Item1);
                _display.DisplayPluginLoadValidationError(pluginList.Item2);
                Environment.Exit(0);
            }
            else
            {
                _gameService = new(Dimension, pluginList.Item1);
                _input = new(Dimension, pluginList.Item1);
                _display = new(pluginList.Item1);
            }
            Dimension = _input.GridSizeInput();
            //_display.DisplayAnimalCount();
            _display.DisplayGameTitle();
            int cursorTop = Console.CursorTop;
            bool isGameRunning = true;
            bool isPredatorTurn = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (isGameRunning)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                _gameService.MoveAnimals(Dimension, grid, ref isPredatorTurn);
                Thread.Sleep(250);

                _input.ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
    }
}
