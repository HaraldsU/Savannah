namespace AnimalLibrary.Models.Animals
{
    public class LionModel : IPlugin
    {
        public char Name { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public float Health { get; set; }
        public int BreedingCooldown { get; set; }
        public int BreedingTime { get; set; }
        public int ActiveBreedingCooldown { get; set; }
        public bool IsBirthing { get; set; }
        public LionModel()
        {
            Name = 'L';
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
