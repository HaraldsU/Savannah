using AnimalLibrary.Models;

namespace ClassLibrary.Services
{
    public class ButtonListenerService
    {
        private readonly GameService _gameService;
        public ButtonListenerService()
        {
            _gameService = new();
        }
        public void ButtonListener(List<GridCellModel> grid)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo click;
                click = Console.ReadKey(true);
                if (click.Key == ConsoleKey.Q)
                    Environment.Exit(0);
                else
                {
                    _gameService.AddAnimal(animal: null, click.Key, grid, isChild: false, updates: null);
                }
            }
        }
    }
}
