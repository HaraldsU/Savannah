using Microsoft.AspNetCore.Mvc;
using SavannaWebAPI.Models;
using ClassLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SavannaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridModelController : ControllerBase
    {
        private GridModelDTO _gridModelDTO = new();
        private readonly GridService _initializeGrid = new();
        // GET: api/<GridModelController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _gridModelDTO.Grid = _initializeGrid.Initialize(5);

            return Ok(_gridModelDTO);
        }


        // GET api/<GridModelController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GridModelController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GridModelController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GridModelController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
