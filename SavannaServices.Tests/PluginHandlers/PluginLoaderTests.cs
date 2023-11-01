using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services.PluginHandlers.Tests
{
    [TestClass()]
    public class PluginLoaderTests
    {
        private GameService _gameService;

        [TestInitialize()]
        public void Initialize(SavannaContext dbContext, CurrentGamesModel currentGames, CurrentSessionModel currentSessions)
        {
            _gameService = new(dbContext, currentGames, currentSessions);
        }

        [TestMethod()]
        public void LoadPluginsTest()
        {
            Assert.IsInstanceOfType(_gameService.Animals, typeof(List<IAnimalProperties>));
        }
    }
}