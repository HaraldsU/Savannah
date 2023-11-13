using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Savanna.Commons.Enums;
using Savanna.Commons.Models;
using Savanna.Data.Models;
using Savanna.Data.Models.Animals;
using Savanna.Data.Models.DB;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
        private readonly int dimensions = 4;
        private readonly int sessionId = 0;
        private readonly int gameId = 1;

        //private SavannaContext? _dbContext;
        private CurrentGamesHolder? _currentGames;
        private GameService? _gameService;
        private List<GridCellModelDTO>? gridDto;

        private Mock<DbSet<GameStateModel>> _mockSet;
        private Mock<SavannaContext> _mockContext;

        [TestInitialize()]
        public void Initialize()
        {
            _mockSet = new Mock<DbSet<GameStateModel>>();
            _mockContext = new Mock<SavannaContext>();
            _mockContext.Setup(m => m.GameState).Returns(_mockSet.Object);

            //_dbContext = A.Fake<SavannaContext>(x => x.WithArgumentsForConstructor(() => new SavannaContext(new DbContextOptions<SavannaContext>())));
            _currentGames = A.Fake<CurrentGamesHolder>();
            _gameService = new GameService(_mockContext.Object, _currentGames);
        }

        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            gridDto = _gameService.AddAnimal(gameId, sessionId, lion, false, new());

            // Assert
            Assert.AreEqual(1, Utilities.GetAnimalCount(gridDto, AnimalTypeEnums.Prey));
            Assert.AreEqual(1, Utilities.GetAnimalCount(gridDto, AnimalTypeEnums.Predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var lionModel = new LionModel();
            var lion = lionModel.Name;
            gridDto = _gameService.AddAnimal(gameId, sessionId, lion, false, new());
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(gridDto);

            // Act
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(gridDto));
        }

        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(gridDto);
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Act
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(gridDto));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            gridDto = _gameService.AddAnimal(gameId, sessionId, lion, false, new());
            var currentGame = _currentGames.Games.Find(g => g.Game.Id == gameId && g.SessionId == sessionId);
            var grid = currentGame.Game.Grid;

            // Assert
            Assert.AreEqual(2, _gameService.GetAnimalCount(grid));
        }

        [TestMethod()]
        public void GetAnimalListTest()
        {
            Assert.IsNotNull(_gameService.GetAnimalList());
        }

        [TestMethod()]
        public void GetAnimalValidationErrorsTest()
        {
            Assert.IsNotNull(_gameService.GetAnimalValidationErrors());
        }

        [TestMethod()]
        public void AddNewGameTest()
        {
            //Act
            _gameService.AddNewGame(dimensions, sessionId);

            //Assert
            Assert.AreEqual(1, _currentGames.Games.Count);
        }

        [TestMethod()]
        public void GetNewSessionIdTest()
        {
            //Act
            _gameService.AddNewGame(dimensions, sessionId);

            //Assert
            Assert.AreEqual(1, _gameService.GetNewSessionId());
        }

        [TestMethod()]
        public void SaveGameTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);

            // Act
            _gameService.SaveGame(gameId, sessionId);

            // Assert
            _mockSet.Verify(m => m.Add(It.IsAny<GameStateModel>()), Moq.Times.Once());
            _mockContext.Verify(m => m.SaveChanges(), Moq.Times.Once());
        }
    }
}