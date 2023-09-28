using AnimalLibrary.Models.Animals;
using ClassLibrary.PluginHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class AnimalMovementTests
    {
        private int Dimensions = 8;
        private GameService _gameService;
        private GridService _grid;
        private AnimalBehaviour _animalMovement;
        private PluginLoader _pluginLoader;

        [TestInitialize()]
        public void Initialize()
        {
            _pluginLoader = new();
            _gameService = new(Dimensions, _pluginLoader.LoadPlugins().Item1);
            _grid = new();
            _animalMovement = new(_gameService);
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
            _gameService.AddAnimal(animalAntelope, pressedKey: ConsoleKey.NoName, grid, isChild);
            _animalMovement.GetAnimalsNewPositions(Dimensions, grid, isPredatorTurn, updates);

            // Assert
            Assert.AreNotEqual(updatesOld, updates);
        }
    }
}