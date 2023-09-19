using ClassLibrary.Constants;
using ClassLibrary.Models;
using System.Formats.Asn1;

namespace ClassLibrary
{
    public class UpdateGame
    {
        public static List<IPlugin>? Plugins;
        private AnimalMovement _animalMovement;
        public UpdateGame(List<IPlugin> plugins)
        {
            Plugins = plugins;
            _animalMovement = new();
        }
        // Adds a new prey or predator depending on input
        public static void AddAnimal(char animalName, List<GridCellModel> grid, bool isChild, Dictionary<int, int>? updates = null)
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
                    if ((grid[cellIndex].Animal != null) || updates.ContainsValue(cellIndex))
                    {
                        do
                        {
                            cellIndex = RandomGenerator.Next(grid.Count);
                            if (grid[cellIndex].Animal == null && isChild == true)
                            {
                                break;
                            }
                        } while ((grid[cellIndex].Animal != null) || updates.ContainsValue(cellIndex));
                    }
                }

                foreach (IPlugin plugin in Plugins)
                {
                    if (animalName == plugin.FirstLetter)
                    {
                        var animal = plugin;
                        var animalModel = new AnimalsModel();
                        if (isChild)
                            animal.ActiveBreedingCooldown = animal.BreedingCooldown;

                        if (plugin.Type == AnimalTypeConstants.prey)
                        {
                            animalModel.Prey = plugin.CreateNewAnimal();
                        }
                        else if (plugin.Type == AnimalTypeConstants.predator)
                        {
                            animalModel.Predator = plugin.CreateNewAnimal(); ;
                        }
                        grid[cellIndex].Animal = animalModel;
                    }
                }
            }
        }
        // Gets the current animal count
        public static int GetAnimalCount(List<GridCellModel> grid, string? type = AnimalTypeConstants.all)
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (type == AnimalTypeConstants.all)
                {
                    if (cell.Animal != null && (cell.Animal.Prey != null || cell.Animal.Predator != null))
                        count++;
                }
                else if (type == AnimalTypeConstants.prey)
                {
                    if (cell.Animal != null && cell.Animal.Prey != null)
                        count++;
                }
                else if (type == AnimalTypeConstants.predator)
                {
                    if (cell.Animal != null && cell.Animal.Predator != null)
                        count++;
                }
            }
            return count;
        }
        // Sets the new animals positions and clears the old ones
        public void MoveAnimals(int dimension, List<GridCellModel> grid, bool turn)
        {
            var updates = new Dictionary<int, int>();
            _animalMovement.GetAnimalsNewPositions(dimension, grid, turn, updates);

            foreach (var update in updates)
            {
                var oldValue = grid[update.Value].Animal;
                // Moving of the animal
                grid[update.Value].Animal = grid[update.Key].Animal;
                if (grid[update.Key].Animal != null)
                {
                    DeleteAnimalNoHealth(grid[update.Key].Animal);
                }
                // If animal moved, empty the previous cell 
                if (grid[update.Value].Animal != oldValue)
                {
                    grid[update.Key].Animal = null;
                }
            }
        }
        private void DeleteAnimalNoHealth(AnimalsModel keyAnimal)
        {
            if (keyAnimal.Predator != null)
            {
                if (keyAnimal.Predator.Health == 0)
                {
                    keyAnimal.Predator = null;
                }
            }
            else if (keyAnimal.Prey != null)
            {
                if (keyAnimal.Prey.Health == 0)
                {
                    keyAnimal.Prey = null;
                }
            }
        }
    }
}
