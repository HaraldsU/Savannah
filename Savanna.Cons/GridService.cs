using Savanna.Commons.Models;
using System.Text;

namespace Savanna.Cons
{
    public class GridService
    {
        public StringBuilder MakeGrid(List<GridCellModelDTO> grid, StringBuilder gridStringBuilder, int dimension)
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
    }
}
