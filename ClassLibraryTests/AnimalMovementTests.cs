using AnimalLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private UpdateGame _updateGame;
        private GridService _grid;
        private AnimalMovement _animalMovement;
        private PluginLoader _pluginLoader;
        private static List<IPlugin> Plugins;

        [TestInitialize()]
        public void Initialize()
        {
            _pluginLoader = new();
            Plugins = _pluginLoader.LoadPlugins();
            _updateGame = new(Plugins);
            _grid = new();
            _animalMovement = new();
        }
        [TestMethod()]
        public void GetAnimalsNewPositionsTest()
        {
            // Arrange
            int dimensions = 8;
            char animalAntelope = 'A';
            var grid = _grid.Initialize(dimensions);
            bool isChild = false;
            bool turn = false;
            Dictionary<int, int> updatesOld = new();
            Dictionary<int, int> updates = new();

            // Act
            UpdateGame.AddAnimal(animalAntelope, grid, isChild);
            _animalMovement.GetAnimalsNewPositions(dimensions, grid, turn, updates);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}