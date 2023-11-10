using Savanna.Commons.Enums;
using Savanna.Commons.Models;

namespace Savanna.Services.Tests
{
    public static class Utilities
    {
        public static GridCellModelDTO GetFirstCellWithAnimal(List<GridCellModelDTO> grid)
        {
            var animalCell = new GridCellModelDTO();
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    animalCell = cell;
                }
            }
            return animalCell;
        }
        public static int GetAnimalCount(List<GridCellModelDTO> grid, AnimalTypeEnums type = AnimalTypeEnums.All)
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
