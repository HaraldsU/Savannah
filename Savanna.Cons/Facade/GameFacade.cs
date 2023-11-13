using Savanna.Commons.Models;
using Savanna.Cons.Handlers;

namespace Savanna.Cons.Facade
{
    public class GameFacade
    {
        private readonly AnimalListHandler _animalListHandler;
        private readonly GameHandler _gameHandler;

        public GameFacade(HttpClient httpClient, Display display)
        {
            _animalListHandler = new(httpClient, display);
            _gameHandler = new(httpClient, display);
        }

        public async Task<List<AnimalBaseDTO>> GetAnimalListAsync()
        {
            return await _animalListHandler.OnGetAnimalListAsync();
        }

        public async Task<string> GetAnimalValidationErrorsAsync()
        {
            return await _animalListHandler.OnGetAnimalValidationErrorsAsync();
        }

        public async Task<List<GridCellModelDTO>> StartGameAsync(int dimensions)
        {
            return await _gameHandler.OnPostStartGameAsync(dimensions);
        }

        public async Task<List<GridCellModelDTO>> AddAnimalAsync(string animalName)
        {
            return await _gameHandler.OnPostAddAnimalAsync(animalName);
        }

        public async Task<List<GridCellModelDTO>> MoveAnimalsAsync()
        {
            return await _gameHandler.OnPostMoveAnimalsAsync();
        }
    }
}
