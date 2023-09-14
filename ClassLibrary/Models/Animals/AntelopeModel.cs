namespace ClassLibrary.Models.Animals
{
    public class AntelopeModel : IAnimal
    {
        public int Speed { get; set; }
        public int Range { get; set; }
        public float Health { get; set; }
        public int BreedingCooldown { get; set; }
        public int BreedingTime { get; set; }
        public int ActiveBreedingCooldown { get; set; }
        public bool IsBirthing { get; set; }
        public AntelopeModel()
        {
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
