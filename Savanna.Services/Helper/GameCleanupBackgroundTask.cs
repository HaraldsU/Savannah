using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Savanna.Commons.Constants;
using Savanna.Data.Models;

namespace Savanna.Services.Helper
{
    public class GameCleanupBackgroundTask : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromMinutes(0.5);
        private CurrentGamesHolder _currentGamesHolder;
        private readonly ILogger<GameCleanupBackgroundTask> _logger;

        public GameCleanupBackgroundTask(CurrentGamesHolder currentGamesHolder, ILogger<GameCleanupBackgroundTask> logger)
        {
            _currentGamesHolder = currentGamesHolder;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new(_period);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await timer.WaitForNextTickAsync(stoppingToken);

                    for (int i = _currentGamesHolder.Games.Count - 1; i >= 0; i--)
                    {
                        var game = _currentGamesHolder.Games[i];
                        if (DateTime.Now - game.Timestamp > _period)
                        {
                            _currentGamesHolder.Games.RemoveAt(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ErrorMessageConstants.BackgroundTaskFailed);
                }
            }
        }
    }
}
