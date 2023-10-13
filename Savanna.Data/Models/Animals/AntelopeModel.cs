using Savanna.Commons;

namespace Savanna.Data.Models.Animals
{
    public class AntelopeModel : AnimalBase
    {
        public AntelopeModel()
        {
            Name = "Antelope";
            FirstLetter = 'A';
            KeyBind = ConsoleKey.A;
            AnimalType = AnimalTypeEnums.Prey;
            Color = "Gray";
            Speed = 1;
            Range = 2;
            Health = 10;
            BreedingCooldown = 4;
            BreedingTime = 2;
        }
        public override IAnimal CreateNewAnimal()
        {
            return new AntelopeModel();
        }
    }
}
