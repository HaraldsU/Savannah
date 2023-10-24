using Figgle;
using Savanna.Commons.Constants;
using Savanna.Commons.Enums;
using Savanna.Commons.Models;
using System.Text;
using System.Text.RegularExpressions;

namespace Savanna.Cons
{
    public class Display
    {
        public void DisplayPluginLoadValidationError(string validationError)
        {
            Console.WriteLine(validationError);
        }
        public void DisplayAnimalCount(List<AnimalBaseDTO> animals)
        {
            var animalModels = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.AnimalModels);
            var files = Directory.GetFiles(animalModels);
            Console.WriteLine($"{animals.Count} Animal(s) found");
            Console.WriteLine($"{animals.Count - files.Length} Plugin(s) found\n");
        }
        public void DisplayGameTitle()
        {
            Console.WriteLine(FiggleFonts.MaxFour.Render("Savannah!"));
        }
        public void DisplayGridSizeInputPrompt()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("Enter Grid Size (int 4 to 10): ");
        }
        public void DisplayGameplayInfo(List<AnimalBaseDTO> animals)
        {
            string article;
            string pattern = @"[AEIOU]";
            bool isVowel;
            foreach (var animal in animals)
            {
                isVowel = Regex.IsMatch(animal.FirstLetter.ToString(), pattern);
                if (isVowel)
                {
                    article = "an";
                }
                else
                {
                    article = "a";
                }
                string animalType = string.Empty;
                switch (animal.AnimalType)
                {
                    case AnimalTypeEnums.Predator:
                        animalType = "Predator";
                        break;
                    case AnimalTypeEnums.Prey:
                        animalType = "Prey";
                        break;
                }

                Console.WriteLine("Press " + "'" + animal.FirstLetter + "'" + " to add " + article + " " + animal.Name + " (" + animalType + ")");
            }
            Console.WriteLine("Press 'Q' to quit ...");
        }

        public void DisplayGrid(List<GridCellModelDTO> grid, List<AnimalBaseDTO> animals, int cursorTop)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, cursorTop - 1);
            StringBuilder gridStringBuilder = new();
            gridStringBuilder = MakeGrid(grid, gridStringBuilder, (int)MathF.Sqrt(grid.Count));

            string pattern = @"^[A-Z]$";
            for (int i = 0; i < gridStringBuilder.Length; i++)
            {
                bool isAnimal = Regex.IsMatch(gridStringBuilder[i].ToString(), pattern);
                if (isAnimal)
                {
                    ChangeColor(gridStringBuilder[i].ToString(), GetColor(animals, gridStringBuilder[i]));
                }
                else
                {
                    ChangeColor(gridStringBuilder[i].ToString(), "Red");
                }
            }
        }
        private StringBuilder MakeGrid(List<GridCellModelDTO> grid, StringBuilder gridStringBuilder, int dimension)
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
                        {
                            gridStringBuilder.Append('-');
                        }
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
                                gridStringBuilder.Append(grid[listIndex].Animal?.FirstLetter);
                            }
                            else
                            {
                                gridStringBuilder.Append(' ');
                            }
                        }
                    }
                }
                count = 0;
                if (i != 0 && i % 2 == 0)
                {
                    listIndex++;
                }
                gridStringBuilder.Append('\n');
            }
            gridStringBuilder.Append('\n');

            return gridStringBuilder;
        }

        public void ChangeColor(string text, string color)
        {
            if (color == "Yellow")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (color == "Dark_yellow")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (color == "Gray")
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (color == "Dark_gray")
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if (color == "Red")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (color == "Dark_red")
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else if (color == "Blue")
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (color == "Dark_blue")
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if (color == "Green")
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (color == "Dark_green")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (color == "Magenta")
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if (color == "Dark_magenta")
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            else if (color == "Cyan")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (color == "Dark_cyan")
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else if (color == "White")
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.Write(text);
            Console.ResetColor();
        }
        private string GetColor(List<AnimalBaseDTO> animals, char firstLetter)
        {
            foreach (var animal in animals)
            {
                if (animal.FirstLetter == firstLetter)
                {
                    return animal.Color;
                }
            }

            return string.Empty;
        }
    }
}
