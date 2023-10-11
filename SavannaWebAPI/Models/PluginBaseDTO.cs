using AnimalLibrary.Models;

namespace SavannaWebAPI.Models
{
    public class PluginBaseDTO
    {
        public string Name { get; set; }
        public char FirstLetter { get; set; }
        public ConsoleKey KeyBind { get; set; }
        public string Color { get; set; }
        public PluginBaseDTO(IPlugin plugin)
        {
            // Map properties from IPlugin to PluginBaseDTO
            Name = plugin.Name;
            FirstLetter = plugin.FirstLetter;
            KeyBind = plugin.KeyBind;
            Color = plugin.Color;
        }
    }
}
