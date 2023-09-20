namespace ClassLibrary.Models.Animals
{
    public class LionModel : IPlugin
    {
        public string Name { get; set; } = "Lion";
        public char FirstLetter { get; set; } = 'L';
        public ConsoleKey KeyBind { get; set; } = ConsoleKey.L;
        public int Type { get; set; } = 0;
        public string Color { get; set; } = "Yellow";
        public int Speed { get; set; } = 2;
        public int Range { get; set; } = 3;
        public float Health { get; set; } = 4f;
        public int BreedingCooldown { get; set; } = 4;
        public int BreedingTime { get; set; } = 3;
        public int ActiveBreedingCooldown { get; set; } = 0;
        public bool IsBirthing { get; set; } = false;
        public IPlugin CreateNewAnimal()
        {
            return new LionModel
            {

            };
        }
    }
}