using Savanna.Commons.Models;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;

namespace Savanna.Services.Helper
{
    public static class ModelConverter
    {
        public static List<GridCellModelDTO> GridCellModelToDto(List<GridCellModel> grid)
        {
            List<GridCellModelDTO> result = new();

            foreach (var gridCell in grid)
            {
                var gridCellDto = new GridCellModelDTO
                {
                    X = gridCell.X,
                    Y = gridCell.Y,
                    Animal = gridCell.Animal != null
                        ? new AnimalBaseDTO()
                        {
                            Name = gridCell.Animal.Name,
                            FirstLetter = gridCell.Animal.FirstLetter,
                            KeyBind = gridCell.Animal.KeyBind,
                            AnimalType = gridCell.Animal.AnimalType,
                            Color = gridCell.Animal.Color
                        }
                        : null
                };

                result.Add(gridCellDto);
            }

            return result;
        }
        public static List<AnimalBaseDTO> AnimalModelToDto(List<IAnimalProperties> animals)
        {
            List<AnimalBaseDTO> result = new();

            foreach (var animal in animals)
            {
                var animalDto = new AnimalBaseDTO()
                {
                    Name = animal.Name,
                    FirstLetter = animal.FirstLetter,
                    KeyBind = animal.KeyBind,
                    AnimalType = animal.AnimalType,
                    Color = animal.Color
                };

                result.Add(animalDto);
            }

            return result;
        }
    }
}
