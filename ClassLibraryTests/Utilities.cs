using AnimalLibrary.Models;

namespace ClassLibraryTests
{
    public static class Utilities
    {
        public static GridCellModel GetFirstCellWithAnimal(List<GridCellModel> grid)
        {
            var animalCell = new GridCellModel();
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    animalCell = cell;
                }
            }
            return animalCell;
        }
    }
}
