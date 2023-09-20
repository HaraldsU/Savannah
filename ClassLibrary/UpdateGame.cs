using ClassLibrary.Constants;
using ClassLibrary.Models;
using System.Formats.Asn1;

namespace ClassLibrary
{
    public class UpdateGame
    {
        public List<IPlugin>? Animals { get; private set; }
        public List<GridCellModel> Grid {  get; private set; }
        private PluginLoader _pluginLoader;
        private GridService _gridService;
        private AnimalBehaviour _animalMovement;
        public UpdateGame(int dimensions)
        {
            _pluginLoader = new();
            _gridService = new GridService();
            _animalMovement = new(this);
            Animals = _pluginLoader.LoadPlugins();
            Grid = _gridService.Initialize(dimensions);
        }
        /// <summary>
        /// Adds a new prey or predator depending on input
        /// </summary>
        /// <param name="animal"></param>
        /// <param name="grid"></param>
        /// <param name="isChild"></param>
        /// <param name="updates"></param>
        public void AddAnimal(IPlugin animal, ConsoleKey pressedKey, List < GridCellModel> grid, bool isChild, Dictionary<int, int>? updates = null)
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

                if (animal == null && pressedKey != ConsoleKey.NoName)
                {
                    foreach (IPlugin plugin in Animals)
                    {
                        if (plugin.KeyBind == pressedKey)
                        {
                            animal = plugin;
                        }
                    }
                }
                var animalModel = new AnimalsModel();
                if (isChild)
                    animal.ActiveBreedingCooldown = animal.BreedingCooldown;

                if (animal.Type == (int)AnimalTypeEnums.prey)
                {
                    animalModel.Prey = animal.CreateNewAnimal();
                }
                else if (animal.Type == (int)AnimalTypeEnums.predator)
                {
                    animalModel.Predator = animal.CreateNewAnimal(); ;
                }
                grid[cellIndex].Animal = animalModel;
            }
        }
        /// <summary>
        /// Gets the current animal count
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetAnimalCount(List<GridCellModel> grid, AnimalTypeEnums type = AnimalTypeEnums.all)
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (type == AnimalTypeEnums.all)
                {
                    if (cell.Animal != null && (cell.Animal.Prey != null || cell.Animal.Predator != null))
                        count++;
                }
                else if (type == AnimalTypeEnums.prey)
                {
                    if (cell.Animal != null && cell.Animal.Prey != null)
                        count++;
                }
                else if (type == AnimalTypeEnums.predator)
                {
                    if (cell.Animal != null && cell.Animal.Predator != null)
                        count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Sets the new animals positions and clears the old ones
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="grid"></param>
        /// <param name="turn"></param>
        public void MoveAnimals(int dimension, List<GridCellModel> grid, ref bool isPredatorTurn)
        {
            var updates = new Dictionary<int, int>();
            _animalMovement.GetAnimalsNewPositions(dimension, grid, isPredatorTurn, updates);

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
            if (isPredatorTurn)
                isPredatorTurn = false;
            else
                isPredatorTurn |= true;
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
        private void KeyMappings(Dictionary<char, ConsoleKey> dictionary)
        {
            dictionary['A'] = ConsoleKey.A;
            dictionary['B'] = ConsoleKey.B;
            dictionary['C'] = ConsoleKey.C;
            dictionary['D'] = ConsoleKey.D;
            dictionary['E'] = ConsoleKey.E;
            dictionary['F'] = ConsoleKey.F;
            dictionary['G'] = ConsoleKey.G;
            dictionary['H'] = ConsoleKey.H;
            dictionary['I'] = ConsoleKey.I;
            dictionary['J'] = ConsoleKey.J;
            dictionary['K'] = ConsoleKey.K;
            dictionary['L'] = ConsoleKey.L;
            dictionary['M'] = ConsoleKey.M;
            dictionary['N'] = ConsoleKey.N;
            dictionary['O'] = ConsoleKey.O;
            dictionary['P'] = ConsoleKey.P;
            dictionary['Q'] = ConsoleKey.Q;
            dictionary['R'] = ConsoleKey.R;
            dictionary['S'] = ConsoleKey.S;
            dictionary['T'] = ConsoleKey.T;
            dictionary['U'] = ConsoleKey.U;
            dictionary['V'] = ConsoleKey.V;
            dictionary['W'] = ConsoleKey.W;
            dictionary['X'] = ConsoleKey.X;
            dictionary['Y'] = ConsoleKey.Y;
            dictionary['Z'] = ConsoleKey.Z;
        }
    }
}
