using AnimalLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private int Dimensions = 8;
        private UpdateGame _updateGame;
        private GridService _grid;
        private AnimalBehaviour _animalMovement;

        [TestInitialize()]
        public void Initialize()
        {
            _updateGame = new(Dimensions);
            _grid = new();
            _animalMovement = new(_updateGame);
        }
        [TestMethod()]
        public void GetAnimalsNewPositionsTest()
        {
            // Arrange
            var animalAntelope = new AntelopeModel();
            var grid = _grid.Initialize(Dimensions);
            bool isChild = false;
            bool isPredatorTurn = false;
            Dictionary<int, int> updatesOld = new();
            Dictionary<int, int> updates = new();

            // Act
            _updateGame.AddAnimal(animalAntelope, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalMovement.GetAnimalsNewPositions(Dimensions, grid, isPredatorTurn, updates);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}