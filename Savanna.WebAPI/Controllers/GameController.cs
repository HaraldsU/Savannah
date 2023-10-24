using Microsoft.AspNetCore.Mvc;
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

        public GameController(SavannaContext dbContext)
        {
            _gameService = new(dbContext);
        }

        // POST: api/Game/PostStartGame
        [HttpPost("PostStartGame")]
        public IActionResult PostStartGame(RequestsModel requestData)
        {
            var dimensions = requestData.Dimensions;
            var gameData = _gameService.AddNewGame((int)dimensions);
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

        // GET: api/Game/GetAnimalPluginList
        [HttpGet("GetAnimalPluginList")]
        public IActionResult GetAnimalPluginList()
        {
            var animalList = _gameService.GetAnimalList();
            if (animalList != null)
            {
                return Ok(animalList);
            }

            return BadRequest();
        }

        // GET: api/Game/GetAnimalValidationErrors
        [HttpGet("GetAnimalValidationErrors")]
        public IActionResult GetAnimalValidationErrors()
        {
            var validationErrors = _gameService.GetAnimalValidationErrors();
            if (validationErrors != null)
            {
                return Ok(validationErrors);
            }

            return BadRequest();
        }

        // POST: api/Game/AddAnimal
        [HttpPost("AddAnimal")]
        public IActionResult AddAnimal(RequestsModel requestData)
        {
            var animal = requestData.AnimalName;
            var id = requestData.GameId;
            if (animal != null && id != null)
            {
                var grid = _gameService.AddAnimal((int)id, animal);
                return Ok(grid);
            }
            return BadRequest();
        }

        // POST: api/Game/MoveAnimals
        [HttpPost("MoveAnimals")]
        public IActionResult MoveAnimals(RequestsModel requestData)
        {
            var id = requestData.GameId;
            if (id != null)
            {
                var grid = _gameService.MoveAnimals((int)id);
                return Ok(grid);
            }
            return BadRequest();
        }

        // POST: api/Game/SaveGame
        [HttpPost("SaveGame")]
        public IActionResult SaveGame(RequestsModel requestData)
        {
            var id = requestData.GameId;
            if (id != null)
            {
                _gameService.SaveGame((int)id);
                return Ok();
            }
            return BadRequest();
        }

        // GET: api/Game/LoadGame
        [HttpPost("LoadGame")]
        public IActionResult LoadGame(RequestsModel requestData)
        {
            var id = requestData.GameId;
            if (id != null)
            {
                var isGame = _gameService.LoadGame((int)id);
                return Ok(isGame);
            }
            return BadRequest();
        }
    }
}