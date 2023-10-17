using Savanna.Commons.Enums;
using Savanna.Data.Base;

namespace Savanna.Data
{
    public class AnimalFactory
    {
        public static AnimalBase CreateAnimal(AnimalTypeEnums animalType)
        {
            switch (animalType)
            {
                case AnimalTypeEnums.Predator:
                    return new PredatorBase();
                case AnimalTypeEnums.Prey:
                    return new PreyBase();
                default:
                    throw new ArgumentException("Unknown animal type");
            }
        }
    }
}
