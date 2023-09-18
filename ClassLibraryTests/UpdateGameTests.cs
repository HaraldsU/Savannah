using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class UpdateGameTests
    {
        private UpdateGame _updateGame;
        private GridService _grid;

        [TestInitialize()]
        public void Initialize()
        {
            _updateGame = new UpdateGame();
            _grid = new GridService();
        }

        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            char animalAntelope = 'A';
            char animalLion = 'L';
            var grid = _grid.Initialize(8);
            bool isChild = false;

            // Act
            _updateGame.AddAnimal(animalAntelope, grid, isChild);
            _updateGame.AddAnimal(animalLion, grid, isChild);

            // Assert
            Assert.AreEqual(1, Utilities.GetAnimalCount(grid, "Antelope"));
            Assert.AreEqual(1, Utilities.GetAnimalCount(grid, "Lion"));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            _updateGame.AddAnimal('L', grid, false);
            bool turn = true;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _updateGame.MoveAnimals(dimensions, grid, turn);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }
        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            _updateGame.AddAnimal('A', grid, false);
            bool turn = false;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _updateGame.MoveAnimals(dimensions, grid, turn);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }
    }
}