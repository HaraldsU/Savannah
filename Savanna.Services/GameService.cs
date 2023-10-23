using Microsoft.EntityFrameworkCore;
using Savanna.Commons.Enums;
using Savanna.Commons.Models;
using Savanna.Data;
using Savanna.Data.Base;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;
using Savanna.Services.Helper;
using Savanna.Services.PluginHandlers;

namespace Savanna.Services
{
    public class GameService
    {
        public readonly List<IAnimalProperties> Animals;
        public readonly string ValidationErrors;

        private readonly AnimalTypeEnums[] animalTypes = (AnimalTypeEnums[])Enum.GetValues(typeof(AnimalTypeEnums));
        private readonly Tuple<List<IAnimalProperties>, string> loadedPlugins;
        private readonly AnimalBehaviour _animalMovement;
        private readonly PluginLoader _pluginLoader;
        private readonly SavannaContext _dbContext;
        private static readonly InitializeService _initializeService = new();

        public GameService(SavannaContext dbContext)
        {
            _pluginLoader = new();
            loadedPlugins = _pluginLoader.LoadPlugins();
            Animals = loadedPlugins.Item1;
            ValidationErrors = loadedPlugins.Item2;

            _animalMovement = new(this);
            _dbContext = dbContext;
        }

        public List<GridCellModel> AddAnimal(int id, string animalName, bool? isChild = false, Dictionary<int, int>? updates = null)
        {
            var grid = _initializeService.Games[id].Grid;
            var animalModel = Animals.Find(animal => animal.Name == animalName);
            var pressedKey = animalModel?.KeyBind;

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

                if (animalModel == null && pressedKey != ConsoleKey.NoName)
                {
                    foreach (IAnimalProperties plugin in Animals)
                    {
                        if (plugin.KeyBind == pressedKey)
                        {
                            animalModel = plugin;
                        }
                    }
                }
                if (animalModel != null)
                {
                    if ((bool)isChild)
                    {
                        animalModel.ActiveBreedingCooldown = animalModel.BreedingCooldown;
                    }
                    var newAnimalModel = animalModel.CreateNewAnimal();
                    grid[cellIndex].Animal = newAnimalModel;
                }
            }

            return grid;
        }
        public List<GridCellModel> MoveAnimals(int id)
        {
            var game = _initializeService.Games.Find(game => game.Id == id);
            var grid = game.Grid;
            var dimension = game.Dimensions;
            var turn = game.Turn;
            var currentTypeIndex = game.CurrentTypeIndex;

            DetermineAnimalTypeTurn(ref turn, ref currentTypeIndex);
            var updatedAnimalPositions = new Dictionary<int, int>();
            _animalMovement.GetAnimalsNewPositions(dimension, grid, turn, updatedAnimalPositions);

            foreach (var update in updatedAnimalPositions)
            {
                MoveAnimal(grid, update);
            }

            game.Grid = grid;
            game.Turn = turn;
            game.CurrentTypeIndex = currentTypeIndex;

            return grid;
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
        public List<AnimalBaseDTO> GetAnimalList()
        {
            var animalList = ModelConverter.AnimalModelToDTO(Animals);
            return animalList;
        }
        public void SaveGame(int id)
        {
            var gameToSave = _initializeService.Games[id];
            var existingGameState = _dbContext.GameState.FirstOrDefault(gs => gs.Id == gameToSave.Id);

            if (existingGameState != null)
            {
                _dbContext.Entry(existingGameState).CurrentValues.SetValues(gameToSave);
            }
            else
            {
                _dbContext.GameState.Add(gameToSave);
            }

            _dbContext.SaveChanges();
        
        }
        public bool LoadGame(int id)
        {
            var gameToLoad = _dbContext.GameState
                .Include(gs => gs.Grid)
                .ThenInclude(grid => grid.Animal)
                .FirstOrDefault(gs => gs.Id == id);
            _initializeService.Games.Add(gameToLoad);

            if (gameToLoad != null)
            {
                return true;
            }
            return false;
        }
        public Tuple<int, List<GridCellModel>> AddNewGame(int dimensions)
        {
            var returnData = _initializeService.InitializeGame(dimensions);
            return returnData;
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
                        AnimalBase animal = AnimalFactory.CreateAnimal(grid[update.Key].Animal.AnimalType);
                        animal.AnimalEatsAnimal();
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
        private void DeleteAnimalNoHealth(List<GridCellModel> grid, int cell)
        {
            grid[cell].Animal = null;
        }
    }
}

