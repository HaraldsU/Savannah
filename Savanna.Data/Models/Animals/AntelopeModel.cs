using Savanna.Commons.Enums;
using Savanna.Data.Base;
using Savanna.Data.Interfaces;

namespace Savanna.Data.Models.Animals
{
    public class AntelopeModel : PreyBase
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
        public override PreyBase CreateNewAnimal()
        {
            return new AntelopeModel();
        }
    }
}
