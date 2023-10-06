namespace SavannaWebApplication.Models
{
    public class PluginBaseDTO
    {
        public string Name { get; set; }
        public char FirstLetter { get; set; }
        public ConsoleKey KeyBind { get; set; }
        public bool IsPrey { get; set; }
        public string Color { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public float Health { get; set; }
        public int BreedingCooldown { get; set; }
        public int BreedingTime { get; set; }
        public int ActiveBreedingCooldown { get; set; }
        public bool IsBirthing { get; set; }
    }
}
