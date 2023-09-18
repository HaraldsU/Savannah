using ClassLibrary.Models.Animals;

namespace ClassLibrary.Models
{
    public class AnimalsModel
    {
        public IPlugin? Predator { get; set; } = null;
        public IPlugin? Prey { get; set; } = null;
    }
}
