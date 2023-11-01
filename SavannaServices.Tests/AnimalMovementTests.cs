using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Data.Models.Animals;
using Savanna.Data.Models.DB;
using Savanna.Services;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private readonly int dimensions = 8;
        private GameService? _gameService;
        private InitializeService? _initializeService;
        private AnimalBehaviour? _animalBehaviour;

        [TestInitialize()]
        public void Initialize(SavannaContext dbContext, CurrentGamesModel currentGames, CurrentSessionModel currentSessions)
        {
            _gameService = new(dbContext, currentGames, currentSessions);
            _initializeService = new();
            _animalBehaviour = new(_gameService);
        }
        [TestMethod()]
        public void GetAnimalsNewPositionsTest()
        {
            // Arrange
            var oldGameState = new GameStateModel();
            var gameId = 1;
            var sessionId = 0;
            var grid = _initializeService.InitializeGame(dimensions, ref oldGameState, gameId).Item2;

            Dictionary<int, int> updatesOld = new();
            Dictionary<int, int> updates = new();

            var animalAntelope = new AntelopeModel();
            var antelope = animalAntelope.Name;
            AnimalTypeEnums turn = AnimalTypeEnums.Predator;

            // Act
            _gameService.AddAnimal(gameId, sessionId, antelope);
            _animalBehaviour.GetAnimalsNewPositions(dimensions, grid, turn, updates, gameId, sessionId);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}