using ClassLibrary;
using AnimalLibrary.Models;
using Figgle;
using AnimalLibrary.Models.Animals;
using System.Text.RegularExpressions;

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
            _updateGame = new UpdateGame(Program._plugins);
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
                string article;
                string pattern = @"[AEIOU]";
                bool isVowel;
                foreach (IPlugin plugin in Program._plugins)
                {
                    isVowel = Regex.IsMatch(plugin.FirstLetter.ToString(), pattern);
                    if (isVowel)
                        article = "an";
                    else
                        article = "a";
                    Console.WriteLine("Press " + "'" + plugin.FirstLetter + "'" + " to add " + article + " " + plugin.Name + " (" + plugin.Type + ")");
                }
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
                else
                {
                    Dictionary<char, ConsoleKey> dictionary = new Dictionary<char, ConsoleKey>();
                    KeyMappings(dictionary);
                    foreach (IPlugin plugin in Program._plugins)
                    {
                        if (click.Key == dictionary.GetValueOrDefault(plugin.FirstLetter))
                        {
                            _updateGame.AddAnimal(plugin.FirstLetter, grid, false);
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
