using Savanna.Commons.Enums;
using Savanna.Data.Models;

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
        public static int GetAnimalCount(List<GridCellModel> grid, AnimalTypeEnums type = AnimalTypeEnums.All)
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    if (type == AnimalTypeEnums.All)
                    {
                        count++;
                    }
                    else if (cell.Animal.AnimalType == type)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
