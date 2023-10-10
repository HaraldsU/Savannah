using AnimalLibrary.Models.Animals;
using ClassLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private readonly int dimensions = 8;
        private GameService? _gameService;
        private GridService? _grid;
        private AnimalBehaviour? _animalMovement;

        [TestInitialize()]
        public void Initialize()
        {
            _gameService = new();
            _grid = new();
            _animalMovement = new(_gameService);
        }
        [TestMethod()]
        public void GetAnimalsNewPositionsTest()
        {
            // Arrange
            var animalAntelope = new AntelopeModel();
            var grid = _grid.Initialize(dimensions);
            bool isChild = false;
            bool isPredatorTurn = false;
            Dictionary<int, int> updatesOld = new();
            Dictionary<int, int> updates = new();

            // Act
            _gameService.AddAnimal(animalAntelope, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalMovement.GetAnimalsNewPositions(dimensions, grid, isPredatorTurn, updates);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}