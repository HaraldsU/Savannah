using ClassLibrary;
using AnimalLibrary.Models;
using Figgle;
using AnimalLibrary.Models.Animals;
using System.Text.RegularExpressions;

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
            _updateGame = new(Program.Plugins);
        }
        public void Run()
        {
            Dimension = _input.GridSizeInput();
            _display.DisplayAnimalCount();
            _display.DisplayGameTitle();
            int cursorTop = Console.CursorTop;
            bool check = true;
            bool turn = true;
            var grid = _initializeGrid.Initialize(Dimension);

            while (check)
            {
                _display.DisplayGrid(grid, cursorTop, Dimension);
                if (turn) // Predator turn
                {
                    _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = false;
                    Thread.Sleep(250);
                }
                else // Prey turn
                {
                    _updateGame.MoveAnimals(Dimension, grid, turn);
                    turn = true;
                    Thread.Sleep(250);
                }
                _input.ButtonListener(grid);
                _display.DisplayGameplayInfo();
            }
        }
    }
}
