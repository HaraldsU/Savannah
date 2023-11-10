using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Models.DB;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class InitializeServiceTests
    {
        private InitializeService? _initializeService;

        [TestInitialize()]
        public void Initialize()
        {
            _initializeService = new();
        }

        [TestMethod()]
        public void InitializeGameTest()
        {
            // Arrange
            int dimension = 8;
            var oldGameState = new GameStateModel();
            int newId = 1;

            // Act
            var game = _initializeService.InitializeGame(dimension, ref oldGameState, newId);

            // Assert
            Assert.IsNotNull(game);
        }
    }
}