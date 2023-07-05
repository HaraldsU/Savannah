using ClassLibrary.Models;
using System.Text;

namespace Savannah
{
    public class Display
    {
        public void DisplayGrid(List<GridCellModel> grid, int dimension)
        {
            Console.CursorVisible = false;
            //Console.SetCursorPosition(0, 0);
            StringBuilder gridStringBuilder = new StringBuilder();
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
                                if (grid[listIndex].Animal.Lion != null)
                                    gridStringBuilder.Append(grid[listIndex].Animal.Lion.Name);
                                else if (grid[listIndex].Animal.Antelope != null)
                                    gridStringBuilder.Append(grid[listIndex].Animal.Antelope.Name);
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
            Console.WriteLine(gridStringBuilder.ToString());
        }
    }
}
