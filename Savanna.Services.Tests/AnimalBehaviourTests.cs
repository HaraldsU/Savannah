using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Commons.Enums;
using Savanna.Commons.Models;
using Savanna.Data.Models;
using Savanna.Data.Models.Animals;
using Savanna.Data.Models.DB;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class AnimalBehaviourTests
    {
        private readonly int dimensions = 4;
        private readonly int sessionId = 0;
        private readonly int gameId = 1;

        private SavannaContext? _dbContext;
        private CurrentGamesHolder? _currentGames;
        private GameService? _gameService;
        private AnimalBehaviour? _animalBehaviour;
        private List<GridCellModelDTO>? gridDto;

        [TestInitialize()]
        public void Initialize()
        {
            _dbContext = A.Fake<SavannaContext>(x => x.WithArgumentsForConstructor(() => new SavannaContext(new DbContextOptions<SavannaContext>())));
            _currentGames = A.Fake<CurrentGamesHolder>();
            _gameService = new GameService(_dbContext, _currentGames);
            _animalBehaviour = new(_gameService);
        }
        [TestMethod()]
        public void GetAnimalsNewPositionsTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            Dictionary<int, int> updatesOld = new();
            Dictionary<int, int> updates = new();
            var animalAntelope = new AntelopeModel();
            var antelope = animalAntelope.Name;
            AnimalTypeEnums turn = AnimalTypeEnums.Predator;

            // Act
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            var currentGame = _currentGames.Games.Find(g => g.Game.Id == gameId && g.SessionId == sessionId);
            var grid = currentGame.Game.Grid;
            _animalBehaviour.GetAnimalsNewPositions(dimensions, grid, turn, updates, gameId, sessionId);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}