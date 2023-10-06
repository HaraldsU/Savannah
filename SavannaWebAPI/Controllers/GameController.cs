using AnimalLibrary.Models;
using ClassLibrary;
using ClassLibrary.PluginHandlers;
using Microsoft.AspNetCore.Mvc;
using SavannaWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static readonly PluginLoader _pluginLoader = new();
        private GridModelDTO? _gridModelDTO;
        private GameService? _gameService;
        private Tuple<List<IPlugin>, string> _animals = _pluginLoader.LoadPlugins();
        private readonly GridService _initializeGrid = new();
        private readonly int dimensions = 10;

        // GET: api/Game/GetGrid
        [HttpGet("GetGrid")]
        public Task<IActionResult> GetGrid()
        {
            _gridModelDTO = new GridModelDTO
            {
                Grid = _initializeGrid.Initialize(dimensions)
            };

            return Task.FromResult<IActionResult>(Ok(_gridModelDTO));
        }
        // GET: api/Game/GetGameService
        [HttpGet("GetGameService")]
        public Task<IActionResult> GetGameService()
        {
            var pluginBaseDTOs = _animals.Item1.Select(plugin => new PluginBaseDTO(plugin)).ToList();
            return Task.FromResult<IActionResult>(Ok(pluginBaseDTOs));
        }
        // POST: api/Game/AddAnimal
        [HttpPost("AddAnimal")]
        public IActionResult AddAnimal(RequestsModel requestData)
        {
            var animal = _animals.Item1.FirstOrDefault(animal => animal.Name == requestData.AnimalName);
            var grid = requestData.Grid;
            _gameService = new GameService(dimensions, _animals.Item1);
            _gameService.AddAnimal(animal: null, animal.KeyBind, grid.Grid, false);
            return Ok(grid);
        }
        // POST: api/Game/MoveAnimals
        [HttpPost("MoveAnimals")]
        public IActionResult MoveAnimals(RequestsModel requestData)
        {
            var isPredatorTurnString = requestData.IsPredatorTurn;
            if (isPredatorTurnString == "false" || isPredatorTurnString == "true")
            {
                bool isPredatorTurn = isPredatorTurnString == "true" ? true : false;
                var gridDTO = requestData.Grid;
                _gameService = new GameService(dimensions, _animals.Item1);
                _gameService.MoveAnimals(dimensions, gridDTO.Grid, ref isPredatorTurn);
                return Ok(gridDTO);
            }
            else
            {
                return BadRequest(123);
            }
        }
    }
}
