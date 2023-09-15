using AnimalLibrary.Models;

namespace ClassLibraryTests
{
    public static class Utilities
    {
        public static int GetAnimalCount(List<GridCellModel> grid, string? type = "All")
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (type == "All")
                {
                    if (cell.Animal != null && (cell.Animal.Antelope != null || cell.Animal.Lion != null))
                        count++;
                }
                else if (type == "Antelope")
                {
                    if (cell.Animal != null && cell.Animal.Antelope != null)
                        count++;
                }
                else if (type == "Lion")
                {
                    if (cell.Animal != null && cell.Animal.Lion != null)
                        count++;
                }
            }
            return count;
        }
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
