namespace ClassLibrary.Models.Animals
{
    public class AntelopeModel : IPlugin
    {
        public string Name { get; set; } = "Antelope";
        public char FirstLetter { get; set; } = 'A';
        public ConsoleKey KeyBind { get; set; } = ConsoleKey.A;
        public int Type { get; set; } = 1;
        public string Color { get; set; } = "Grey";
        public int Speed { get; set; } = 1;
        public int Range { get; set; } = 2;
        public float Health { get; set; } = 6f;
        public int BreedingCooldown { get; set; } = 4;
        public int BreedingTime { get; set; } = 3;
        public int ActiveBreedingCooldown { get; set; } = 0;
        public bool IsBirthing { get; set; } = false;
        public IPlugin CreateNewAnimal()
        {
            return new AntelopeModel
            {

            };
        }
    }
}