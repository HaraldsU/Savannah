using ClassLibrary;
using ClassLibrary.Models;
using Savannah;

namespace Savanna.cons
{
    public class Input
    {
        private UpdateGame _updateGame;
        private Display _display;
        public Input(int dimensions)
        {
            _updateGame = new(dimensions);
            _display = new(_updateGame.Animals);
        }
        /// <summary>
        /// Gets grid dimension size from the user.
        /// </summary>
        /// <returns></returns>
        public int GridSizeInput()
        {
            string warningWrongType = "Size needs to be an integer";
            string warningOutOfBounds = "Size needs to be in the interval from 4 to 10";

            do
            {
                _display.DisplayGridSizeInputPrompt();
                var dimensions = Console.ReadLine();
                if (!int.TryParse(dimensions, out _))
                {
                    Console.Clear();
                    _display.DisplayGridSizeInputPrompt();
                    Console.SetCursorPosition(Console.GetCursorPosition().Left, 1);
                    _display.ChangeColor(warningWrongType, "Red");
                }
                else
                {
                    if (Int32.Parse(dimensions) >= 4 && Int32.Parse(dimensions) <= 10)
                    {
                        Console.Clear();
                        return Int32.Parse(dimensions);
                    }
                    else
                    {
                        Console.Clear();
                        _display.DisplayGridSizeInputPrompt();
                        Console.SetCursorPosition(Console.GetCursorPosition().Left, 1);
                        _display.ChangeColor(warningOutOfBounds, "Red");
                    }
                }
            } while (true);
        }
        public void ButtonListener(List<GridCellModel> grid)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo click;
                click = Console.ReadKey(true);
                if ((click.Key == ConsoleKey.Q))
                    Environment.Exit(0);
                else
                {
                    _updateGame.AddAnimal(animal: null, click.Key, grid, isChild: false, updates: null);
                }
            }
        }
    }
}
