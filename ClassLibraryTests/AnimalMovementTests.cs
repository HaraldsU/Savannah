using ClassLibrary.Models;
using ClassLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private UpdateGame _updateGame;
        private GridService _grid;
        private AnimalMovement _animalMovement;
        public PluginLoader _pluginLoader;
        public static List<IPlugin> Plugins;

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