using ClassLibrary.Models;

namespace ClassLibrary
{
    public class GridService
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
