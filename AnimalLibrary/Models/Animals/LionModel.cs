namespace AnimalLibrary.Models.Animals
{
    public class LionModel : PluginBase
    {
        public LionModel()
        {
            Name = "Lion";
            FirstLetter = 'L';
            KeyBind = ConsoleKey.L;
            IsPrey = false;
            Color = "Yellow";
            Speed = 2;
            Range = 3;
            Health = 8;
            BreedingCooldown = 4;
            BreedingTime = 2;
        }
        public override IPlugin CreateNewAnimal()
        {
            return new LionModel();
        }
    }
}
