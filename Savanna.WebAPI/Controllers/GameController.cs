using Microsoft.AspNetCore.Mvc;
using Savanna.Services;
using SavannaWebAPI.Models;

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly GameService _gameService = new();
        private readonly InitializeService _gridService;
        private readonly int dimensions = 4;

        public GameController()
        {
            _gridService = new(_gameService);
        }

        // GET: api/Game/GetInitializedGrid
        [HttpGet("GetInitializedGrid")]
        public IActionResult GetInitializedGrid()
        {
            var gameData = _gridService.InitializeGame(dimensions);
            var gameId = gameData.Item1;
            var grid = gameData.Item2;

            if (grid != null)
            {
                var response = new
                {
                    Id = gameId,
                    Grid = grid
                };

                return Ok(response);
            }
            return BadRequest();
        }

        // GET: api/Game/GetGameService
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
    }
}