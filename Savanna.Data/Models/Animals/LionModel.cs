using Savanna.Commons;

namespace Savanna.Data.Models.Animals
{
    public class LionModel : AnimalBase
    {
        public LionModel()
        {
            Name = "Lion";
            FirstLetter = 'L';
            KeyBind = ConsoleKey.L;
            AnimalType = AnimalTypeEnums.Predator;
            Color = "Yellow";
            Speed = 2;
            Range = 3;
            Health = 8;
            BreedingCooldown = 4;
            BreedingTime = 2;
        }
        public override IAnimal CreateNewAnimal()
        {
            return new LionModel();
        }
    }
}
