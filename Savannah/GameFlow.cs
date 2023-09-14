using ClassLibrary;
using ClassLibrary.Models;
using Figgle;

namespace Savannah
{
    public class GameFlow
    {
        static readonly int Dimension = 4;
        private readonly InitializeGrid _initializeGrid;
        private readonly Display _display;
        private readonly UpdateGame _updateGame;
        public GameFlow()
        {
            _initializeGrid = new InitializeGrid();
            _display = new Display();
            _updateGame = new UpdateGame();
        }
        public void Run()
        {
            Console.WriteLine(FiggleFonts.MaxFour.Render("Savannah!"));
            int cursorTop = Console.CursorTop;
            bool check = true;
            bool turn = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (check)
            {
                _display.DisplayGrid(grid, Dimension, cursorTop);
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
                Console.WriteLine("Press 'L' to add a Lion ...");
                Console.WriteLine("Press 'A' to add an Antelope ...");
                Console.WriteLine("Press 'Q' to quit ...");
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
