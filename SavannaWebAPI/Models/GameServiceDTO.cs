using SavannaWebAPI.Models;

namespace SavannaWebApplication.Models
{
    public class GameServiceDTO
    {
        public GameServiceDTO(int dimensions, List<PluginBaseDTO> animals)
        {
            this.Dimensions = dimensions;
            this.Animals = animals;
        }

        public int Dimensions { get; set; }
        public List<PluginBaseDTO> Animals { get; set; }
    }
}
