namespace AnimalLibrary.Models.Animals
{
    public class AntelopeModel : IPlugin
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
        public AntelopeModel()
        {
            Name = "Antelope";
            FirstLetter = 'A';
            Type = "Prey";
            Color = "Grey";
            Speed = 1;
            Range = 2;
            Health = 6;
            BreedingCooldown = 4;
            BreedingTime = 2;
            ActiveBreedingCooldown = 0;
            IsBirthing = false;
        }
    }
}
