using AnimalLibrary.Models.Animals;
using ClassLibrary.Constants;
using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalFinalizerTests
    {
        private int Dimensions = 8;
        private AnimalFinalizer _animalFinalizer;
        private GridService _grid;

        [TestInitialize()]
        public void Initialize()
        {
            _animalFinalizer = new AnimalFinalizer(Dimensions);
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
            _animalFinalizer.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalFinalizer.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(1, _animalFinalizer.GetAnimalCount(grid, AnimalTypeEnums.prey));
            Assert.AreEqual(1, _animalFinalizer.GetAnimalCount(grid, AnimalTypeEnums.predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            var animalLionModel = new LionModel();
            bool isChild = false;
            _animalFinalizer.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            bool isPredatorTurn = true;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _animalFinalizer.MoveAnimals(dimensions, grid, ref isPredatorTurn);

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
            _animalFinalizer.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            bool isPredatorTurn = false;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _animalFinalizer.MoveAnimals(dimensions, grid, ref isPredatorTurn);

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
            _animalFinalizer.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalFinalizer.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(2, _animalFinalizer.GetAnimalCount(grid, AnimalTypeEnums.all));
        }
    }
}