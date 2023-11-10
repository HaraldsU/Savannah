﻿using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Commons.Enums;
using Savanna.Commons.Models;
using Savanna.Data.Models;
using Savanna.Data.Models.Animals;
using Savanna.Data.Models.DB;

namespace Savanna.Services.Tests
{
    [TestClass()]
    public class GameServiceTests
    {
        private readonly int dimensions = 4;
        private readonly int sessionId = 0;
        private readonly int gameId = 1;

        private SavannaContext? _dbContext;
        private CurrentGamesHolder? _currentGames;
        private GameService? _gameService;
        private List<GridCellModelDTO>? gridDto;

        [TestInitialize()]
        public void Initialize()
        {
            _dbContext = A.Fake<SavannaContext>(x => x.WithArgumentsForConstructor(() => new SavannaContext(new DbContextOptions<SavannaContext>())));
            _currentGames = A.Fake<CurrentGamesHolder>();
            _gameService = new GameService(_dbContext, _currentGames);
        }

        [TestMethod()]
        public void AddAnimalTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            gridDto = _gameService.AddAnimal(gameId, sessionId, lion, false, new());

            // Assert
            Assert.AreEqual(1, Utilities.GetAnimalCount(gridDto, AnimalTypeEnums.Prey));
            Assert.AreEqual(1, Utilities.GetAnimalCount(gridDto, AnimalTypeEnums.Predator));
        }

        [TestMethod()]
        public void MoveAnimalLionTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var lionModel = new LionModel();
            var lion = lionModel.Name;
            gridDto =  _gameService.AddAnimal(gameId, sessionId, lion, false, new());
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(gridDto);

            // Act
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(gridDto));
        }

        [TestMethod()]
        public void MoveAnimalAntelopeTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            var animalOldPosition = Utilities.GetFirstCellWithAnimal(gridDto);
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Act
            gridDto = _gameService.MoveAnimals(gameId, sessionId);

            // Assert
            Assert.AreNotEqual(animalOldPosition, Utilities.GetFirstCellWithAnimal(gridDto));
        }

        [TestMethod()]
        public void GetAnimalCountTest()
        {
            // Arrange
            _gameService.AddNewGame(dimensions, sessionId);
            var antelopeModel = new AntelopeModel();
            var antelope = antelopeModel.Name;
            var lionModel = new LionModel();
            var lion = lionModel.Name;

            // Act
            gridDto = _gameService.AddAnimal(gameId, sessionId, antelope, false, new());
            gridDto = _gameService.AddAnimal(gameId, sessionId, lion, false, new());

            // Assert
            Assert.AreEqual(2, Utilities.GetAnimalCount(gridDto, AnimalTypeEnums.All));
        }
    }
}