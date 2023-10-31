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
        public readonly List<IAnimalProperties>? Animals;
        public readonly string ValidationErrors;

        private readonly AnimalTypeEnums[] animalTypes = (AnimalTypeEnums[])Enum.GetValues(typeof(AnimalTypeEnums));
        private readonly Tuple<List<IAnimalProperties>, string> loadedPlugins;
        private readonly AnimalBehaviour _animalMovement;
        private readonly PluginLoader _pluginLoader;
        private readonly InitializeService _initializeService;
        private readonly SavannaContext _dbContext;
        private CurrentGamesModel? _currentGames;
        private CurrentSessionModel? _currentSessions;

        public GameService(SavannaContext dbContext, CurrentGamesModel currentGames, CurrentSessionModel currentSessions)
        {
            _pluginLoader = new();
            loadedPlugins = _pluginLoader.LoadPlugins();
            Animals = loadedPlugins.Item1;
            ValidationErrors = loadedPlugins.Item2;

            _animalMovement = new(this);
            _initializeService = new();
            _dbContext = dbContext;
            _currentGames = currentGames;
            _currentSessions = currentSessions;
        }

        public List<GridCellModelDTO> AddAnimal(int gameId, int sessionId, string animalName, bool? isChild = false, Dictionary<int, int>? updates = null)
        {
            if (_currentGames.Games.Count != 0)
            {
                var grid = _currentGames.Games.Find(g => g.Game.Id == gameId && g.SessionId == sessionId).Game.Grid;
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

                return ModelConverter.GridCellModelToDTO(grid);
            }

            return new();
        }
        public List<GridCellModelDTO> MoveAnimals(int gameId, int sessionId)
        {
            var currentGame = _currentGames.Games.Find(g => g.Game.Id == gameId && g.SessionId == sessionId);
            var grid = currentGame.Game.Grid;
            var dimension = currentGame.Game.Dimensions;
            var turn = currentGame.Game.Turn;
            var currentTypeIndex = currentGame.Game.CurrentTypeIndex;

            DetermineAnimalTypeTurn(ref turn, ref currentTypeIndex);
            var updatedAnimalPositions = new Dictionary<int, int>();
            _animalMovement.GetAnimalsNewPositions(dimension, grid, turn, updatedAnimalPositions, gameId, sessionId);

            foreach (var update in updatedAnimalPositions)
            {
                MoveAnimal(grid, update);
            }

            currentGame.Game.Grid = grid;
            currentGame.Game.Turn = turn;
            currentGame.Game.CurrentTypeIndex = currentTypeIndex;

            return ModelConverter.GridCellModelToDTO(grid);
        }

        public List<AnimalBaseDTO> GetAnimalList()
        {
            var animalList = ModelConverter.AnimalModelToDTO(Animals);

            return animalList;
        }
        public string GetAnimalValidationErrors()
        {
            return ValidationErrors;
        }

        public bool SaveGame(int gameId, int sessionId)
        {
            try
            {
                if (_currentGames.Games.Count != 0)
                {
                    var gameToSave = _currentGames.Games.Find(g => g.Game.Id == gameId && g.SessionId == sessionId);
                    var gameToSaveCopy = new GameStateModel
                    {
                        Id = 0,
                        Grid = gameToSave.Game.Grid,
                        Turn = gameToSave.Game.Turn,
                        CurrentTypeIndex = gameToSave.Game.CurrentTypeIndex,
                        Dimensions = gameToSave.Game.Dimensions
                    };
                    var existingGameState = _dbContext.GameState.FirstOrDefault(gs => gs.Id == gameToSaveCopy.Id);

                    if (existingGameState != null)
                    {
                        _dbContext.Entry(existingGameState).CurrentValues.SetValues(gameToSaveCopy);
                    }
                    else
                    {
                        _dbContext.GameState.Add(gameToSaveCopy);
                    }

                    int affectedRows = _dbContext.SaveChanges();
                    return affectedRows > 0;
                }
                return false;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool LoadGame(int gameId, int sessionId)
        {
            var gameToLoad = _dbContext.GameState
                .Include(gs => gs.Grid)
                .ThenInclude(grid => grid.Animal)
                .FirstOrDefault(gs => gs.Id == gameId);

            if (gameToLoad != null)
            {
                // Remove old game
                var currentGame = _currentGames.Games.Find(g => g.SessionId == sessionId);
                if (currentGame != null)
                {
                    _currentGames.Games.Remove(currentGame);
                }

                // Add new game
                var newGame = new CurrentGameModel
                {
                    Game = gameToLoad,
                    SessionId = sessionId
                };
                _currentGames.Games.Add(newGame);
                return true;
            }

            return false;
        }
        public Tuple<int, List<GridCellModel>> AddNewGame(int dimensions, int sessionId)
        {
            // Remove old game
            var currentGame = _currentGames.Games.Find(g => g.SessionId == sessionId);
            if (currentGame != null)
            {
                _currentGames.Games.Remove(currentGame);
            }

            // Add new game
            GameStateModel newGameState = new();
            var newId = GetNewGameId();
            var returnData = _initializeService.InitializeGame(dimensions, ref newGameState, newId);

            var newGame = new CurrentGameModel
            {
                Game = newGameState,
                SessionId = sessionId
            };
            _currentGames.Games.Add(newGame);

            return returnData;
        }
        public int CreateNewSessionId()
        {
            if (_currentSessions.Sessions.Count == 0)
            {
                _currentSessions.Sessions.Add(1);
                return 1;
            }
            else
            {
                var lastSession = _currentSessions.Sessions[_currentSessions.Sessions.Count - 1];
                _currentSessions.Sessions.Add(lastSession + 1);
                return lastSession + 1;
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
        private int GetNewGameId()
        {
            if (_currentGames.Games.Count == 0)
            {
                return 1;
            }
            else
            {
                var lastGame = _currentGames.Games[_currentGames.Games.Count - 1];
                return lastGame.Game.Id + 1;
            }
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

