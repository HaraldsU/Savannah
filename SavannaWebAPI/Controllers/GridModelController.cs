using Microsoft.AspNetCore.Mvc;
using SavannaWebAPI.Models;
using ClassLibrary;
using ClassLibrary.PluginHandlers;
using AnimalLibrary.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridModelController : ControllerBase
    {
        private readonly GridModelDTO _gridModelDTO = new();
        private readonly GridService _initializeGrid = new();
        private GameServiceDTO _gameServiceDTO;
        private static readonly PluginLoader _pluginLoader = new();
        private Tuple<List<IPlugin>, string> _animals = _pluginLoader.LoadPlugins();
        private readonly int dimensions = 5;
        // GET: api/<GridModel/GetGrid>
        [HttpGet("GetGrid")]
        public async Task<IActionResult> GetGrid()
        {
            _gridModelDTO.Grid = _initializeGrid.Initialize(dimensions);

            return Ok(_gridModelDTO);
        }
        // GET: api/<GridModel/GetGameService>
        [HttpGet("GetGameService")]
        public async Task<IActionResult> GetGameService()
        {
            _gameServiceDTO = new GameServiceDTO(dimensions, _animals.Item1);
            return Ok(_gameServiceDTO);
        }


        //// GET api/<GridModelController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<GridModelController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<GridModelController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<GridModelController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
