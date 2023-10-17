using Savanna.Data.Base;
using Savanna.Data.Models;
using SavannaWebAPI.Models;

namespace SavannaWebAPI.Helper
{
    public static class ModelConverter
    {
        public static List<GridCellModel> GridCellModelDtoToGridCellModel(List<GridCellModelDTO> grid)
        {
            List<GridCellModel> result = new();

            foreach (var gridCellDto in grid)
            {
                var gridCell = new GridCellModel
                {
                    X = gridCellDto.X,
                    Y = gridCellDto.Y,
                    Animal = gridCellDto.Animal != null
                        ? new AnimalBase
                        {
                            Name = gridCellDto.Animal.Name,
                            FirstLetter = gridCellDto.Animal.FirstLetter,
                            KeyBind = gridCellDto.Animal.KeyBind,
                            AnimalType = gridCellDto.Animal.AnimalType,
                            Color = gridCellDto.Animal.Color,
                            Speed = gridCellDto.Animal.Speed,
                            Range = gridCellDto.Animal.Range,
                            Health = gridCellDto.Animal.Health,
                            BreedingCooldown = gridCellDto.Animal.BreedingCooldown,
                            BreedingTime = gridCellDto.Animal.BreedingTime,
                            ActiveBreedingCooldown = gridCellDto.Animal.ActiveBreedingCooldown,
                            IsBirthing = gridCellDto.Animal.IsBirthing
                        }
                        : null
                };

                result.Add(gridCell);
            }

            return result;
        }
        public static List<GridCellModelDTO> GridCellModelToGridCellModelDto(List<GridCellModel> grid)
        {
            List<GridCellModelDTO> result = new();

            foreach (var gridCell in grid)
            {
                var gridCellDto = new GridCellModelDTO
                {
                    X = gridCell.X,
                    Y = gridCell.Y,
                    Animal = gridCell.Animal != null
                        ? new AnimalBaseDTO(gridCell.Animal)
                        {
                            Name = gridCell.Animal.Name,
                            FirstLetter = gridCell.Animal.FirstLetter,
                            KeyBind = gridCell.Animal.KeyBind,
                            AnimalType = gridCell.Animal.AnimalType,
                            Color = gridCell.Animal.Color,
                            Speed = gridCell.Animal.Speed,
                            Range = gridCell.Animal.Range,
                            Health = gridCell.Animal.Health,
                            BreedingCooldown = gridCell.Animal.BreedingCooldown,
                            BreedingTime = gridCell.Animal.BreedingTime,
                            ActiveBreedingCooldown = gridCell.Animal.ActiveBreedingCooldown,
                            IsBirthing = gridCell.Animal.IsBirthing
                        }
                        : null
                };

                result.Add(gridCellDto);
            }

            return result;
        }

    }
}
