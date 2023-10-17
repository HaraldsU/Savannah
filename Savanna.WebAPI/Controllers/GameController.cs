using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Services;
using SavannaWebAPI.Helper;
using SavannaWebAPI.Models;

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static List<GridCellModel> Grid { get; set; }
        private AnimalTypeEnums turn;
        private int currentTypeIndex;
        private readonly GameService? _gameService;
        private readonly GridService _initializeGrid = new();
        private readonly int dimensions = 4;

        public GameController()
        {
            _gameService = new();
        }

        // GET: api/Game/GetGrid
        [HttpGet("GetGrid")]
        public Task<IActionResult> GetGrid()
        {
            Grid = _initializeGrid.Initialize(dimensions);
            var gridCellModelDTO = ModelConverter.GridCellModelToGridCellModelDto(Grid);
            var gridModelDTO = new GridModelDTO
            {
                Grid = gridCellModelDTO
            };
            return Task.FromResult<IActionResult>(Ok(gridModelDTO));
        }
        // GET: api/Game/GetGameService
        [HttpGet("GetAnimalPluginList")]
        public Task<IActionResult> GetAnimalPluginList()
        {
            var animalBaseDTOs = _gameService.Animals.Select(animal => new AnimalBaseDTO(animal)).ToList();
            return Task.FromResult<IActionResult>(Ok(animalBaseDTOs));
        }
        // POST: api/Game/AddAnimal
        [HttpPost("AddAnimal")]
        public IActionResult AddAnimal(RequestsModel requestData)
        {
            var animal = _gameService.Animals.FirstOrDefault(animal => animal.Name == requestData.AnimalName);
            var gridString = requestData.Grid;
            var gridDTO = JsonConvert.DeserializeObject<GridModelDTO>(gridString);
            if (animal != null && gridString != null && _gameService != null)
            {
                Grid = ModelConverter.GridCellModelDtoToGridCellModel(gridDTO.Grid);
                _gameService.AddAnimal(animal: null, animal.KeyBind, Grid, false);
                gridDTO.Grid = ModelConverter.GridCellModelToGridCellModelDto(Grid);
                return Ok(gridDTO);
            }
            return BadRequest();
        }
        // POST: api/Game/MoveAnimals
        [HttpPost("MoveAnimals")]
        public IActionResult MoveAnimals(RequestsModel requestData)
        {
            var gridString = requestData.Grid;
            var gridDTO = JsonConvert.DeserializeObject<GridModelDTO>(gridString);
            turn = (AnimalTypeEnums)requestData.Turn;
            currentTypeIndex = (int)requestData.CurrentTypeIndex;
            if (gridString != null && _gameService != null)
            {
                Grid = ModelConverter.GridCellModelDtoToGridCellModel(gridDTO.Grid);
                _gameService.MoveAnimals(dimensions, Grid, ref turn, ref currentTypeIndex);
                gridDTO.Grid = ModelConverter.GridCellModelToGridCellModelDto(Grid);

                var response = JsonConvert.SerializeObject(new
                {
                    Grid,
                    Turn = (int)turn,
                    CurrentTypeIndex = currentTypeIndex
                });

                return Ok(response);
            }
            return BadRequest();
        }
    }
}