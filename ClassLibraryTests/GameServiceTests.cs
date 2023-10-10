using AnimalLibrary.Models.Animals;
using ClassLibrary.Constants;
using ClassLibrary.Services;
using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
        private readonly int dimensions = 8;
        private GameService? _gameService;
        private GridService? _grid;

        [TestInitialize()]
        public void Initialize()
        {
            _gameService = new();
            _grid = new();
        }


        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            var animalAntelopeModel = new AntelopeModel();
            var animalLionModel = new LionModel();
            var grid = _grid.Initialize(dimensions);
            bool isChild = false;

            // Act
            _gameService.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _gameService.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(1, _gameService.GetAnimalCount(grid, AnimalTypeEnums.Prey));
            Assert.AreEqual(1, _gameService.GetAnimalCount(grid, AnimalTypeEnums.Predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            var grid = _grid.Initialize(dimensions);
            var animalLionModel = new LionModel();
            bool isChild = false;
            _gameService.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _gameService.MoveAnimals(dimensions, grid);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }
        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            var grid = _grid.Initialize(dimensions);
            var animalAntelopeModel = new AntelopeModel();
            bool isChild = false;
            _gameService.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _gameService.MoveAnimals(dimensions, grid);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            var animalAntelopeModel = new AntelopeModel();
            var animalLionModel = new LionModel();
            var grid = _grid.Initialize(dimensions);
            bool isChild = false;

            // Act
            _gameService.AddAnimal(animalAntelopeModel, pressedKey: ConsoleKey.NoName, grid, isChild);
            _gameService.AddAnimal(animalLionModel, pressedKey: ConsoleKey.NoName, grid, isChild);

            // Assert
            Assert.AreEqual(2, _gameService.GetAnimalCount(grid, AnimalTypeEnums.All));
        }
    }
}