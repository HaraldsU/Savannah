using AnimalLibrary.Models;
using ClassLibrary;
using ClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using SavannaWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService? _gameService;
        private GridModelDTO? _gridModelDTO;
        private readonly List<IPlugin> _animals = AnimalListSingleton.Instance.GetAnimalList();
        private readonly GridService _initializeGrid = new();
        private readonly int dimensions = 10;

        public GameController()
        {
            _gameService = new();
        }

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
        [HttpGet("GetAnimalPluginList")]
        public Task<IActionResult> GetAnimalPluginList()
        {
            var pluginBaseDTOs = _animals.Select(plugin => new PluginBaseDTO(plugin)).ToList();
            return Task.FromResult<IActionResult>(Ok(pluginBaseDTOs));
        }
        // POST: api/Game/AddAnimal
        [HttpPost("AddAnimal")]
        public IActionResult AddAnimal(RequestsModel requestData)
        {
            var animal = _animals.FirstOrDefault(animal => animal.Name == requestData.AnimalName);
            var grid = requestData.Grid;
            _gameService.AddAnimal(animal: null, animal.KeyBind, grid.Grid, false);
            return Ok(grid);
        }
        // POST: api/Game/MoveAnimals
        [HttpPost("MoveAnimals")]
        public IActionResult MoveAnimals(RequestsModel requestData)
        {
            var gridDTO = requestData.Grid;
            _gameService.MoveAnimals(dimensions, gridDTO.Grid);
            return Ok(gridDTO);
        }
    }
}
