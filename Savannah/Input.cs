using ClassLibrary;
using ClassLibrary.Models;
using Savannah;

namespace Savanna.cons
{
    public class Input
    {
        private Display _display;
        public Input() 
        { 
            _display = new();
        }
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
                    Dictionary<char, ConsoleKey> dictionary = new Dictionary<char, ConsoleKey>();
                    KeyMappings(dictionary);
                    foreach (var plugin in Program.Plugins)
                    {
                        if (click.Key == dictionary.GetValueOrDefault(plugin.FirstLetter))
                        {
                            UpdateGame.AddAnimal(plugin.FirstLetter, grid, false);
                        }
                    }
                }
            }
        }
        private void KeyMappings(Dictionary<char, ConsoleKey> dictionary)
        {
            dictionary['A'] = ConsoleKey.A;
            dictionary['B'] = ConsoleKey.B;
            dictionary['C'] = ConsoleKey.C;
            dictionary['D'] = ConsoleKey.D;
            dictionary['E'] = ConsoleKey.E;
            dictionary['F'] = ConsoleKey.F;
            dictionary['G'] = ConsoleKey.G;
            dictionary['H'] = ConsoleKey.H;
            dictionary['I'] = ConsoleKey.I;
            dictionary['J'] = ConsoleKey.J;
            dictionary['K'] = ConsoleKey.K;
            dictionary['L'] = ConsoleKey.L;
            dictionary['M'] = ConsoleKey.M;
            dictionary['N'] = ConsoleKey.N;
            dictionary['O'] = ConsoleKey.O;
            dictionary['P'] = ConsoleKey.P;
            dictionary['Q'] = ConsoleKey.Q;
            dictionary['R'] = ConsoleKey.R;
            dictionary['S'] = ConsoleKey.S;
            dictionary['T'] = ConsoleKey.T;
            dictionary['U'] = ConsoleKey.L;
            dictionary['V'] = ConsoleKey.V;
            dictionary['W'] = ConsoleKey.W;
            dictionary['X'] = ConsoleKey.X;
            dictionary['Y'] = ConsoleKey.Y;
            dictionary['Z'] = ConsoleKey.Z;
        }
    }
}
