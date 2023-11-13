using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services.Helper.Tests
{
    [TestClass()]
    public class GameCleanupBackgroundTaskTests
    {
        private SavannaContext? _dbContext;
        private CurrentGamesHolder? _currentGames;
        private GameService? _gameService;

        private GameCleanupBackgroundTask? _gameCleanupBackgroundTask;
        private ILogger<GameCleanupBackgroundTask> _logger;

        [TestInitialize()]
        public void Initialize()
        {
            _dbContext = A.Fake<SavannaContext>(x => x.WithArgumentsForConstructor(() => new SavannaContext(new DbContextOptions<SavannaContext>())));
            _currentGames = A.Fake<CurrentGamesHolder>();
            _gameService = new GameService(_dbContext, _currentGames);

            _logger = A.Fake<ILogger<GameCleanupBackgroundTask>>();
            _gameCleanupBackgroundTask = new(_currentGames, _logger);
        }

        [TestMethod]
        public async Task GameCleanupBackgroundTask_CleansUpGames()
        {
            // Arrange
            _gameService.AddNewGame(4, 0);
            var initialGameCount = _currentGames.Games.Count;

            // Act
            await _gameCleanupBackgroundTask.StartAsync(CancellationToken.None);
            await Task.Delay(TimeSpan.FromMinutes(0.5));
            await _gameCleanupBackgroundTask.StopAsync(CancellationToken.None);

            // Assert
            Assert.AreNotEqual(initialGameCount, _currentGames.Games.Count);
        }
    }
}