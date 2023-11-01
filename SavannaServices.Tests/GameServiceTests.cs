using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Data.Models.Animals;
using Savanna.Data.Models.DB;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
        private readonly int dimensions = 8;
        private GameService? _gameService;
        private InitializeService? _initializeService;

        [TestInitialize()]
        public void Initialize(SavannaContext dbContext, CurrentGamesModel currentGames, CurrentSessionModel currentSessions)
        {
            _gameService = new(dbContext, currentGames, currentSessions);
            _initializeService = new();
        }

        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            var oldGameState = new GameStateModel();
            int gameId = 1;
            int sessionId = 0;
            var grid = _initializeService.InitializeGame(dimensions, ref oldGameState, gameId).Item2;

            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            _gameService.AddAnimal(gameId, sessionId, antelope);
            _gameService.AddAnimal(gameId, sessionId, lion);

            // Assert
            Assert.AreEqual(1, Utilities.GetAnimalCount(grid, AnimalTypeEnums.Prey));
            Assert.AreEqual(1, Utilities.GetAnimalCount(grid, AnimalTypeEnums.Predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            var oldGameState = new GameStateModel();
            int gameId = 1;
            int sessionId = 0;
            var grid = _initializeService.InitializeGame(dimensions, ref oldGameState, gameId).Item2;

            var lionModel = new LionModel();
            var lion = lionModel.Name;
            _gameService.AddAnimal(gameId, sessionId, lion);

            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }

        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            var oldGameState = new GameStateModel();
            int gameId = 1;
            int sessionId = 0;
            var grid = _initializeService.InitializeGame(dimensions, ref oldGameState, gameId).Item2;

            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            _gameService.AddAnimal(gameId, sessionId, antelope);

            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);
            _gameService.MoveAnimals(gameId, sessionId);

            // Act
            _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            var oldGameState = new GameStateModel();
            int gameId = 1;
            int sessionId = 0;
            var grid = _initializeService.InitializeGame(dimensions, ref oldGameState, gameId).Item2;

            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            _gameService.AddAnimal(gameId, sessionId, antelope);
            _gameService.AddAnimal(gameId, sessionId, lion);

            // Assert
            Assert.AreEqual(2, Utilities.GetAnimalCount(grid, AnimalTypeEnums.All));
        }
    }
}