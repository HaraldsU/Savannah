using ClassLibrary.Models;
using System.Xml.Linq;

namespace ClassLibraryTests
{
    public static class Utilities
    {
        public static int GetAnimalCount(List<GridCellModel> grid, string? name = "All")
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (name == "All")
                {
                    if (cell.Animal != null && (cell.Animal.Prey != null || cell.Animal.Predator != null))
                        count++;
                }
                else if (name == "Antelope")
                {
                    if (cell.Animal != null && cell.Animal.Prey != null && cell.Animal.Prey.Name == "Antelope")
                        count++;
                }
                else if (name == "Lion")
                {
                    if (cell.Animal != null && cell.Animal.Predator != null && cell.Animal.Predator.Name == "Lion")
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
