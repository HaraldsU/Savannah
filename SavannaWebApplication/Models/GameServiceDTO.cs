using AnimalLibrary.Models;

namespace SavannaWebApplication.Models
{
    public class GameServiceDTO
    {
        public GameServiceDTO(int dimensions, List<IPlugin> animals)
        {
            this.Dimensions = dimensions;
            this.Animals = animals;
        }
        public int Dimensions { get; }
        public List<IPlugin> Animals { get; }
    }
}
