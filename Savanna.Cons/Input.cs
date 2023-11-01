using Savanna.Commons.Models;
using Savanna.Cons.Facade;

namespace Savanna.Cons
{
    public class Input
    {
        private readonly Display _display;
        public Input()
        {
            _display = new();
        }
        ///// <summary>
        ///// Gets grid dimension size from the user.
        ///// </summary>
        ///// <returns></returns>
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
        public async Task<List<GridCellModelDTO>> ButtonListener(GameFacade apiRequests, List<AnimalBaseDTO> animals)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo click;
                click = Console.ReadKey(true);

                if (click.Key == ConsoleKey.Q)
                {
                    Environment.Exit(0);
                }
                else
                {
                    var animal = animals.FirstOrDefault(x => x.KeyBind == click.Key);
                    if (animal != null)
                    {
                        var grid = await apiRequests.AddAnimalAsync(animal.Name);
                        return grid;
                    }
                }
            }

            return new List<GridCellModelDTO>();
        }

    }
}
