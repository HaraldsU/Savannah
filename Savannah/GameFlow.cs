using ClassLibrary;
using ClassLibrary.Models;
using System.Threading.Tasks;

namespace Savannah
{
    public class GameFlow
    {
        static readonly int Dimension = 6;
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
            bool check = true;
            bool turn = true;
            int count = 0;
            var grid = _initializeGrid.Initialize(Dimension);
            while (check)
            {
                _display.DisplayGrid(grid, Dimension);
                if (turn)
                {
                    _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = false;
                    Thread.Sleep(250);
                }
                else
                {
                   _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = true;
                    Thread.Sleep(250);
                }
                ButtonListener(grid);
                count++;
                //if (count % 4 == 0) Console.Clear();
            }
        }
        private void ButtonListener(List<GridCellModel> grid)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo click;
                click = Console.ReadKey(true);
                if (click.Key == ConsoleKey.A)
                {
                    _updateGame.AddAnimal('A', grid);
                }
                else if (click.Key == ConsoleKey.L)
                {
                    _updateGame.AddAnimal('L', grid);
                }
            }
        }
    }
}
