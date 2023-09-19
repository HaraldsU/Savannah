using AnimalLibrary.Models.Animals;
using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savannah;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class UpdateGameTests
    {
        private UpdateGame _updateGame;
        private GridService _grid;
        private PluginLoader _pluginLoader;
        private static List<IPlugin> Plugins;

        [TestInitialize()]
        public void Initialize()
        {
            _pluginLoader = new();
            Plugins = _pluginLoader.LoadPlugins();
            _updateGame = new UpdateGame(Plugins);
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
            UpdateGame.AddAnimal(animalAntelope, grid, isChild);
            UpdateGame.AddAnimal(animalLion, grid, isChild);

            // Assert
            Assert.AreEqual(1, UpdateGame.GetAnimalCount(grid, AnimalTypeConstants.prey));
            Assert.AreEqual(1, UpdateGame.GetAnimalCount(grid, AnimalTypeConstants.predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            int dimensions = 8;
            var grid = _grid.Initialize(dimensions);
            char animalCharacter = 'L';
            bool isChild = false;
            UpdateGame.AddAnimal(animalCharacter, grid, isChild);
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
            char animalCharacter = 'A';
            bool isChild = false;
            UpdateGame.AddAnimal(animalCharacter, grid, isChild);
            bool turn = false;
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(grid);

            // Act
            _updateGame.MoveAnimals(dimensions, grid, turn);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(grid));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            char animalAntelopeCharacter = 'A';
            char animalLionCharacter = 'L';
            var grid = _grid.Initialize(8);
            bool isChild = false;

            // Act
            UpdateGame.AddAnimal(animalAntelopeCharacter, grid, isChild);
            UpdateGame.AddAnimal(animalLionCharacter, grid, isChild);

            // Assert
            Assert.AreEqual(2, UpdateGame.GetAnimalCount(grid, AnimalTypeConstants.all));
        }
    }
}