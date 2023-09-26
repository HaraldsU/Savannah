namespace AnimalLibrary.Models.Animals
{
    public class AntelopeModel : PluginBase
    {
        public AntelopeModel()
        {
            Name = "Antelope";
            FirstLetter = 'A';
            KeyBind = ConsoleKey.A;
            IsPrey = true;
            Color = "Gray";
            Speed = 1;
            Range = 2;
            Health = 6f;
            BreedingCooldown = 4;
            BreedingTime = 2;
        }
        public override IPlugin CreateNewAnimal()
        {
            return new AntelopeModel();
        }
    }
}
