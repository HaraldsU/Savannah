using ClassLibrary.Models;
using Figgle;
using System.Text;
using System.Text.RegularExpressions;

namespace Savannah
{
    public class Display
    {
        private List<IPlugin>? Animals;
        public Display(List<IPlugin> plugins)
        {
            Animals = plugins;
        }

        public void DisplayAnimalCount()
        {
            Console.WriteLine($"{Animals.Count} animal(s) found\n");
        }
        public void DisplayGridSizeInputPrompt()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("Enter Grid Size (int 4 to 10): ");
        }
        public void DisplayGameTitle()
        {
            Console.WriteLine(FiggleFonts.MaxFour.Render("Savannah!"));
        }
        public void DisplayGameplayInfo()
        {
            string article;
            string pattern = @"[AEIOU]";
            bool isVowel;
            foreach (var plugin in Animals)
            {
                isVowel = Regex.IsMatch(plugin.FirstLetter.ToString(), pattern);
                if (isVowel)
                    article = "an";
                else
                    article = "a";
                string animalType = string.Empty;
                if (plugin.IsPrey == Convert.ToBoolean(0))
                    animalType = "Predator";
                else if (plugin.IsPrey == Convert.ToBoolean(1))
                    animalType = "Prey";

                Console.WriteLine("Press " + "'" + plugin.FirstLetter + "'" + " to add " + article + " " + plugin.Name + " (" + animalType + ")");
            }
            Console.WriteLine("Press 'Q' to quit ...");
        }
        public void DisplayGrid(List<GridCellModel> grid, int cursorTop, int dimension)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, cursorTop - 1);
            StringBuilder gridStringBuilder = new();
            gridStringBuilder = MakeGrid(grid, gridStringBuilder, dimension);

            string pattern = @"^[A-Z]$";
            for (int i = 0; i < gridStringBuilder.Length; i++)
            {
                bool isAnimal = Regex.IsMatch(gridStringBuilder[i].ToString(), pattern);
                if (isAnimal)
                    ChangeColor(gridStringBuilder[i].ToString(), GetColor(gridStringBuilder[i]));
                else
                    ChangeColor(gridStringBuilder[i].ToString(), "Red");
            }
        }
        public void ChangeColor(string text, string color)
        {
            if (color == "Yellow")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (color == "Dark_yellow")
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (color == "Gray")
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (color == "Dark_gray")
                Console.ForegroundColor = ConsoleColor.DarkGray;
            else if (color == "Red")
                Console.ForegroundColor = ConsoleColor.Red;
            else if (color == "Dark_red")
                Console.ForegroundColor = ConsoleColor.DarkRed;
            else if (color == "Blue")
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (color == "Dark_blue")
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            else if (color == "Green")
                Console.ForegroundColor = ConsoleColor.Green;
            else if (color == "Dark_green")
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            else if (color == "Magenta")
                Console.ForegroundColor = ConsoleColor.Magenta;
            else if (color == "Dark_magenta")
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else if (color == "Cyan")
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (color == "Dark_cyan")
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            else if (color == "White")
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ResetColor();
        }
        private string GetColor(char firstLetter)
        {
            foreach (var plugin in Animals)
            {
                if (plugin.FirstLetter == firstLetter)
                    return plugin.Color;
            }
            return string.Empty;
        }
        private StringBuilder MakeGrid(List<GridCellModel> grid, StringBuilder gridStringBuilder, int dimension)
        {
            int height = (dimension * 2) + 1;
            int width = (dimension * 3) + 2;
            int count = 0;
            int listIndex = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j != width - 1)
                            gridStringBuilder.Append('-');
                    }
                    else
                    {
                        if (j % 3 == 0)
                        {
                            gridStringBuilder.Append('|');
                            count++;
                        }
                        else if (j + 1 != width)
                        {
                            if (count == 2)
                            {
                                listIndex++;
                                count = 1;
                            }
                            if (grid[listIndex].Animal?.Predator != null)
                                gridStringBuilder.Append(grid[listIndex].Animal?.Predator.FirstLetter);
                            else if (grid[listIndex].Animal?.Prey != null)
                                gridStringBuilder.Append(grid[listIndex].Animal?.Prey.FirstLetter);
                            else
                                gridStringBuilder.Append(' ');
                        }
                    }
                }
                count = 0;
                if (i != 0 && i % 2 == 0)
                    listIndex++;
                gridStringBuilder.Append('\n');
            }
            gridStringBuilder.Append('\n');
            return gridStringBuilder;
        }
    }
}
