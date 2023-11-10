using Microsoft.AspNetCore.Mvc;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;
using Savanna.Services;
using SavannaWebAPI.Models;

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(SavannaContext savannaContext, CurrentGamesHolder currentGames)
        {
            _gameService = new(savannaContext, currentGames);
        }

        // GET: api/Game/AnimalPluginList
        [HttpGet("AnimalList")]
        public IActionResult OnGetAnimalList()
        {
            var animalList = _gameService.GetAnimalList();

            if (animalList != null)
            {
                return Ok(animalList);
            }
            return BadRequest();
        }
        // GET: api/Game/AnimalValidationErrors
        [HttpGet("AnimalValidationErrors")]
        public IActionResult OnGetAnimalValidationErrors()
        {
            var validationErrors = _gameService.GetAnimalValidationErrors();

            if (validationErrors != null)
            {
                return Ok(validationErrors);
            }
            return BadRequest();
        }
        // GET: api/Game/SessionId
        [HttpGet("SessionId")]
        public IActionResult OnGetCreateNewSessionId()
        {
            var sessionId = _gameService.GetNewSessionId();

            if (sessionId != null)
            {
                return Ok(sessionId);
            }
            return BadRequest();
        }

        // POST: api/Game/StartGame
        [HttpPost("StartGame")]
        public IActionResult OnPostStartGame(RequestsModel requestData)
        {
            var dimensions = requestData.Dimensions;
            var sessionId = requestData.SessionId;
            var gameData = _gameService.AddNewGame((int)dimensions, (int) sessionId);

            var gameId = gameData.Item1;
            var grid = gameData.Item2;

            if (grid != null)
            {
                var response = new
                {
                    GameId = gameId,
                    Grid = grid
                };
                return Ok(response);
            }
            return BadRequest();
        }
        // POST: api/Game/AddAnimal
        [HttpPost("AddAnimal")]
        public IActionResult OnPostAddAnimal(RequestsModel requestData)
        {
            var animal = requestData.AnimalName;
            var gameId = requestData.GameId;
            var sessionId = requestData.SessionId;
            var isInAnimalList = _gameService.Animals.Any(a => a.Name == animal);

            if (animal != null && gameId != null && sessionId != null && isInAnimalList)
            {
                var grid = _gameService.AddAnimal((int)gameId, (int)sessionId, animal, false, new());
                return Ok(grid);
            }
            return BadRequest();
        }
        // POST: api/Game/MoveAnimals
        [HttpPost("MoveAnimals")]
        public IActionResult OnPostMoveAnimals(RequestsModel requestData)
        {
            var gameId = requestData.GameId;
            var sessionId = requestData.SessionId;

            if (gameId != null && sessionId != null)
            {
                var grid = _gameService.MoveAnimals((int)gameId, (int)sessionId);
                return Ok(grid);
            }
            return BadRequest();
        }
        // POST: api/Game/LoadGame
        [HttpPost("LoadGame")]
        public IActionResult OnPostLoadGame(RequestsModel requestData)
        {
            var gameId = requestData.GameId;
            var sessionId = requestData.SessionId;

            if (gameId != null && sessionId != null)
            {
                var isGame = _gameService.LoadGame((int)gameId, (int)sessionId);
                return Ok(isGame);
            }
            return BadRequest();
        }
        // POST: api/Game/SaveGame
        [HttpPost("SaveGame")]
        public IActionResult OnPostSaveGame(RequestsModel requestData)
        {
            var gameId = requestData.GameId;
            var sessionId = requestData.SessionId;

            if (gameId != null && sessionId != null)
            {
                var isSaved = _gameService.SaveGame((int)gameId, (int)sessionId);
                return Ok(isSaved);
            }
            return BadRequest();
        }
    }
}