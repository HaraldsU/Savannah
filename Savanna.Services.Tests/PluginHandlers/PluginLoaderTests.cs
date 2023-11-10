using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services.PluginHandlers.Tests
{
    [TestClass()]
    public class PluginLoaderTests
    {
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
        public void LoadPluginsTest()
        {
            Assert.IsInstanceOfType(_gameService.Animals, typeof(List<IAnimalProperties>));
        }
    }
}