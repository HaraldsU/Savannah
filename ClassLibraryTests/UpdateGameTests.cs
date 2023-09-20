using ClassLibrary;
using ClassLibrary.Constants;
using ClassLibrary.Models;
using ClassLibrary.Models.Animals;
using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class UpdateGameTests
    {
        private int Dimensions = 8;
        private UpdateGame _updateGame;
        private GridService _grid;

        [TestInitialize()]
        public void Initialize()
        {
            _updateGame = new UpdateGame(Dimensions);
            _grid = new GridService();
        }

        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            var animalAntelopeModel = new AntelopeModel();
            var animalLionModel = new LionModel();
            var grid = _grid.Initialize(8);
            bool isChild = false;

            // Act
            _updateGame.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _updateGame.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(1, _updateGame.GetAnimalCount(grid, AnimalTypeEnums.prey));
            Assert.AreEqual(1, _updateGame.GetAnimalCount(grid, AnimalTypeEnums.predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            var animalLionModel = new LionModel();
            bool isChild = false;
            _updateGame.AddAnimal(animalLionModel, pressedKey : ConsoleKey.NoName, grid, isChild);
            bool isPredatorTurn = true;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _updateGame.MoveAnimals(dimensions, grid, ref isPredatorTurn);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }
        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            var animalAntelopeModel = new AntelopeModel();
            bool isChild = false;
            _updateGame.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            bool isPredatorTurn = false;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _updateGame.MoveAnimals(dimensions, grid, ref isPredatorTurn);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            var animalAntelopeModel = new AntelopeModel();
            var animalLionModel = new LionModel();
            var grid = _grid.Initialize(8);
            bool isChild = false;

            // Act
            _updateGame.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _updateGame.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(2, _updateGame.GetAnimalCount(grid, AnimalTypeEnums.all));
        }
    }
}