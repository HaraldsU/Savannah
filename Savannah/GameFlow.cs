using ClassLibrary;
using ClassLibrary.Models;
using Savanna.cons;

namespace Savannah
{
    public class GameFlow
    {
        public static int Dimension;
        private readonly GridService _initializeGrid;
        private readonly Display _display;
        private readonly Input _input;
        private readonly UpdateGame _updateGame;
        public GameFlow()
        {
            _initializeGrid = new();
            _display = new();
            _input = new();
            _updateGame = new();
        }
        public void Run()
        {
            Dimension = _input.GridSizeInput();
            _display.DisplayGameTitle();
            int cursorTop = Console.CursorTop;
            bool check = true;
            bool turn = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (check)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                if (turn) // Lion turn
                {
                    _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = false;
                    Thread.Sleep(250);
                }
                else // Antelope turn
                {
                    _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = true;
                    Thread.Sleep(250);
                }
                ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
        private void ButtonListener(List<GridCellModel> grid)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo click;
                click = Console.ReadKey(true);
                if (click.Key == ConsoleKey.A)
                    _updateGame.AddAnimal('A', grid, false);
                else if (click.Key == ConsoleKey.L)
                    _updateGame.AddAnimal('L', grid, false);
                else if ((click.Key == ConsoleKey.Q))
                    Environment.Exit(0);
            }
        }
    }
}
