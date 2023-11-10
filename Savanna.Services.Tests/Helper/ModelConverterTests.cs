using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Models.DB;
using Savanna.Data.Models;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Savanna.Data.Models.Animals;

namespace Savanna.Services.Helper.Tests
{
    [TestClass()]
    public class ModelConverterTests
    {
        private readonly int dimensions = 4;
        private readonly int sessionId = 0;

        private SavannaContext? _dbContext;
        private CurrentGamesHolder? _currentGames;
        private GameService? _gameService;

        [TestInitialize()]
        public void Initialize()
        {
            _dbContext = A.Fake<SavannaContext>(x => x.WithArgumentsForConstructor(() => new SavannaContext(new DbContextOptions<SavannaContext>())));
            _currentGames = A.Fake<CurrentGamesHolder>();
            _gameService = new GameService(_dbContext, _currentGames);
        }

        [TestMethod()]
        public void GridCellModelToDtoTest()
        {
            // Arrange
            var grid = _gameService.AddNewGame(dimensions, sessionId).Item2;

            // Act
            var gridDto = ModelConverter.GridCellModelToDto(grid);

            // Assert
            Assert.AreNotEqual(gridDto, grid);
        }

        [TestMethod()]
        public void AnimalModelToDtoTest()
        {
            // Arrange
            var animalList = _gameService.Animals;

            // Act
            var animalDto = ModelConverter.AnimalModelToDto(animalList);

            // Assert
            Assert.AreNotEqual(animalDto, animalList);
        }
    }
}