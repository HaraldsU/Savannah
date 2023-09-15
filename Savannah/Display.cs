using AnimalLibrary.Models;
using AnimalLibrary;
using System.Text;
using AnimalLibrary.Models.Animals;
using System.Text.RegularExpressions;

namespace Savannah
{
    public class Display
    {
        public void DisplayGrid(List<GridCellModel> grid, int dimension, int cursorTop)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, cursorTop - 1);
            StringBuilder gridStringBuilder = new();
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
                            if (grid[listIndex].Animal != null)
                            {
                                if (grid[listIndex].Animal.Predator != null)
                                    gridStringBuilder.Append(grid[listIndex].Animal.Predator.FirstLetter);
                                else if (grid[listIndex].Animal.Prey != null)
                                    gridStringBuilder.Append(grid[listIndex].Animal.Prey.FirstLetter);
                                else
                                    gridStringBuilder.Append(' ');
                            }
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
            string pattern = @"^[A-Z]$";
            for (int i = 0; i < gridStringBuilder.Length; i++)
            {
                bool isAnimal = Regex.IsMatch(gridStringBuilder[i].ToString(), pattern);
                if (isAnimal)
                {
                    ChangeColor(gridStringBuilder[i].ToString(), GetColor(gridStringBuilder[i]));
                }
                //if (gridStringBuilder[i] == 'A')
                //    ChangeColor(gridStringBuilder[i].ToString(), "gray");
                //else if (gridStringBuilder[i] == 'L')
                //    ChangeColor(gridStringBuilder[i].ToString(), "yellow");
                else
                    ChangeColor(gridStringBuilder[i].ToString(), "Red");
            }
        }
        private string GetColor(char firstLetter)
        {
            foreach (IPlugin plugin in Program._plugins)
            {
                if (plugin.FirstLetter == firstLetter)
                {
                    return plugin.Color;
                }
            }
            return string.Empty;
        }
        private void ChangeColor(string text, string color)
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
    }
}
