using AnimalLibrary.Models;
using System.Text;

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
                                if (grid[listIndex].Animal.Lion != null)
                                    gridStringBuilder.Append('L');
                                else if (grid[listIndex].Animal.Antelope != null)
                                    gridStringBuilder.Append('A');
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
        private void ChangeColor(string text, string color)
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
    }
}
