using Savanna.Commons.Models;
using Savanna.Cons.Handlers;

namespace Savanna.Cons.Facade
{
    public class GameFacade
    {
        private readonly Display _display;
        private readonly AnimalActionHandler _animalActionHandler;
        private readonly AnimalListHandler _animalListHandler;
        private readonly GameManagerHandler _gameManager;

        public GameFacade(HttpClient httpClient, Display display)
        {
            _display = display;
            _animalActionHandler = new(httpClient, display);
            _animalListHandler = new(httpClient, display);
            _gameManager = new(httpClient, display);
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
            return await _gameManager.OnPostStartGameAsync(dimensions);
        }

        public async Task<List<GridCellModelDTO>> AddAnimalAsync(string animalName)
        {
            return await _animalActionHandler.OnPostAddAnimalAsync(animalName);
        }

        public async Task<List<GridCellModelDTO>> MoveAnimalsAsync()
        {
            return await _animalActionHandler.OnPostMoveAnimalsAsync();
        }
    }
}
