using Savanna.Commons.Enums;
using Savanna.Data.Base;

namespace Savanna.Data.Interfaces
{
    public interface IAnimalProperties
    {
        string Name { get; set; }
        char FirstLetter { get; set; }
        ConsoleKey KeyBind { get; set; }
        AnimalTypeEnums AnimalType { get; set; }
        string Color { get; set; }
        int Speed { get; set; }
        int Range { get; set; }
        float Health { get; set; }
        int BreedingCooldown { get; set; }
        int BreedingTime { get; set; }
        int ActiveBreedingCooldown { get; set; }
        bool IsBirthing { get; set; }
        AnimalBase CreateNewAnimal();
    }
}