using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private int Dimensions = 8;
        private AnimalFinalizer _animalFinalizer;
        private GridService _grid;
        private AnimalBehaviour _animalMovement;

        [TestInitialize()]
        public void Initialize()
        {
            _animalFinalizer = new(Dimensions);
            _grid = new();
            _animalMovement = new(_animalFinalizer);
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
            _animalFinalizer.AddAnimal(animalAntelope, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalMovement.GetAnimalsNewPositions(Dimensions, grid, isPredatorTurn, updates);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}