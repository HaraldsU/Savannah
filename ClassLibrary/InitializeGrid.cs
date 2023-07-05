using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class InitializeGrid
    {
        public List<GridCellModel> Initialize(int dimension)
        {
            var cells = new List<GridCellModel>();
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var cell = new GridCellModel();
                    cell.Y = i;
                    cell.X = j;
                    cells.Add(cell);
                }
            }
            return cells;
        }
    }
}
