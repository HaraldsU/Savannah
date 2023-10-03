using SavannaWebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SavannaWebApplication.Models
{
    public class GameServiceDTO
    {
        public GameServiceDTO()
        {
        }

        public GameServiceDTO(int dimensions, List<PluginBaseDTO> animals)
        {
            Dimensions = dimensions;
            Animals = animals;
        }

        public int Dimensions { get; set; }
        public List<PluginBaseDTO> Animals { get; set; }
    }
}
