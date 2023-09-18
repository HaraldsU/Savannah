using ClassLibrary.Models;
using Figgle;
using System.Text;

namespace Savannah
{
    public class Display
    {
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
            Console.WriteLine("Press 'L' to add a Lion ...");
            Console.WriteLine("Press 'A' to add an Antelope ...");
            Console.WriteLine("Press 'Q' to quit ...");
        }
        public void DisplayGrid(List<GridCellModel> grid, int cursorTop, int dimension)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, cursorTop - 1);
            StringBuilder gridStringBuilder = new();
            gridStringBuilder = MakeGrid(grid, gridStringBuilder, dimension);
            for (int i = 0; i < gridStringBuilder.Length; i++)
            {
                if (gridStringBuilder[i] == 'A')
                    ChangeColor(gridStringBuilder[i].ToString(), "gray");
                else if (gridStringBuilder[i] == 'L')
                    ChangeColor(gridStringBuilder[i].ToString(), "yellow");
                else
                    ChangeColor(gridStringBuilder[i].ToString(), "red");
            }
        }
        public void ChangeColor(string text, string color)
        {
            if (color == "yellow")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (color == "gray")
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (color == "red")
                Console.ForegroundColor = ConsoleColor.Red;
            else if (color == "white")
                Console.ForegroundColor = ConsoleColor.White;
            Console.Write(text);
            Console.ResetColor();
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
                            if (grid[listIndex].Animal?.Lion != null)
                                gridStringBuilder.Append('L');
                            else if (grid[listIndex].Animal?.Antelope != null)
                                gridStringBuilder.Append('A');
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
