using Savanna.Commons.Enums;
using Savanna.Data.Base;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;
using Savanna.Services.PluginHandlers;

namespace Savanna.Services
{
    public class GameService
    {
        private readonly AnimalTypeEnums[] animalTypes = (AnimalTypeEnums[])Enum.GetValues(typeof(AnimalTypeEnums));
        private readonly AnimalBehaviour _animalMovement;
        private readonly PluginLoader _pluginLoader;
        private readonly Tuple<List<IAnimalProperties>, string> loadedPlugins;
        public readonly List<IAnimalProperties> Animals;
        public readonly string ValidationErrors;
        public GameService()
        {
            _pluginLoader = new();
            _animalMovement = new(this);
            loadedPlugins = _pluginLoader.LoadPlugins();
            Animals = loadedPlugins.Item1;
            ValidationErrors = loadedPlugins.Item2;
        }

        public void AddAnimal(IAnimalProperties animal, ConsoleKey pressedKey, List<GridCellModel> grid, bool isChild, Dictionary<int, int>? updates = null)
        {
            var cellIndex = RandomGenerator.Next(grid.Count);
            var animalCount = GetAnimalCount(grid);

            if (animalCount < grid.Count)
            {
                if (updates == null)
                {
                    if (grid[cellIndex].Animal != null)
                    {
                        do
                        {
                            cellIndex = RandomGenerator.Next(grid.Count);
                        } while (grid[cellIndex].Animal != null);
                    }
                }
                else
                {
                    if (grid[cellIndex].Animal != null || updates.ContainsValue(cellIndex))
                    {
                        do
                        {
                            cellIndex = RandomGenerator.Next(grid.Count);
                            if (grid[cellIndex].Animal == null && isChild == true)
                            {
                                break;
                            }
                        } while (grid[cellIndex].Animal != null || updates.ContainsValue(cellIndex));
                    }
                }

                if (animal == null && pressedKey != ConsoleKey.NoName)
                {
                    foreach (IAnimalProperties plugin in Animals)
                    {
                        if (plugin.KeyBind == pressedKey)
                        {
                            animal = plugin;
                        }
                    }
                }
                if (animal != null)
                {
                    if (isChild)
                    {
                        animal.ActiveBreedingCooldown = animal.BreedingCooldown;
                    }
                    var animalModel = animal.CreateNewAnimal();
                    grid[cellIndex].Animal = animalModel;
                }
            }
        }
        public void MoveAnimals(int dimension, List<GridCellModel> grid, ref AnimalTypeEnums turn, ref int currentTypeIndex)
        {
            DetermineAnimalTypeTurn(ref turn, ref currentTypeIndex);
            var updatedAnimalPositions = new Dictionary<int, int>();
            _animalMovement.GetAnimalsNewPositions(dimension, grid, turn, updatedAnimalPositions);

            foreach (var update in updatedAnimalPositions)
            {
                MoveAnimal(grid, update);
            }
        }
        private void DetermineAnimalTypeTurn(ref AnimalTypeEnums turn, ref int currentTypeIndex)
        {
            if (currentTypeIndex == animalTypes.Length - 1)
            {
                currentTypeIndex = 0;
            }
            AnimalTypeEnums nextType = animalTypes[(currentTypeIndex) % animalTypes.Length];
            turn = nextType;
            currentTypeIndex = (currentTypeIndex + 1) % animalTypes.Length;
        }
        private void MoveAnimal(List<GridCellModel> grid, KeyValuePair<int, int> update)
        {
            var oldPosition = grid[update.Value].Animal;

            // Delete animal if health <= 0
            if (grid[update.Key].Animal.Health <= 0)
            {
                DeleteAnimalNoHealth(grid, update.Key);
            }
            // Move the animal
            else
            {
                if (grid[update.Value].Animal != null)
                {
                    if (grid[update.Key].Animal.AnimalType != grid[update.Value].Animal.AnimalType)
                    {
                        grid[update.Key].Animal.AnimalEatsAnimal();
                    }
                }
                grid[update.Value].Animal = grid[update.Key].Animal;
            }

            // If animal moved, empty the previous cell 
            if (grid[update.Value].Animal != oldPosition)
            {
                grid[update.Key].Animal = null;
            }
        }
        public int GetAnimalCount(List<GridCellModel> grid)
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    count++;
                }
            }
            return count;
        }
        private void DeleteAnimalNoHealth(List<GridCellModel> grid, int cell)
        {
            grid[cell].Animal = null;
        }
    }
}
