namespace AnimalLibrary.Models.Animals
{
    public class LionModel : IPlugin
    {
        public string Name { get; }
        public char FirstLetter { get; }
        public string Type { get; }
        public string Color { get; }
        public int Speed { get; }
        public int Range { get; }
        public float Health { get; set; }
        public int BreedingCooldown { get; set; }
        public int BreedingTime { get; set; }
        public int ActiveBreedingCooldown { get; set; }
        public bool IsBirthing { get; set; }
        public LionModel()
        {
            Name = "Lion";
            FirstLetter = 'L';
            Type = "Predator";
            Color = "Yellow";
            Speed = 2;
            Range = 3;
            Health = 4;
            BreedingCooldown = 4;
            BreedingTime = 2;
            ActiveBreedingCooldown = 0;
            IsBirthing = false;
        }
    }
}
