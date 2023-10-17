using Savanna.Commons.Enums;
using Savanna.Data.Interfaces;

namespace SavannaWebAPI.Models
{
    public class AnimalBaseDTO
    {
        public string Name { get; set; }
        public char FirstLetter { get; set; }
        public ConsoleKey KeyBind { get; set; }
        public AnimalTypeEnums AnimalType { get; set; }
        public string Color { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
        public float Health { get; set; }
        public int BreedingCooldown { get; set; }
        public int BreedingTime { get; set; }
        public int ActiveBreedingCooldown { get; set; }
        public bool IsBirthing { get; set; }
        public AnimalBaseDTO()
        {

        }
        public AnimalBaseDTO(IAnimalProperties plugin)
        {
            Name = plugin.Name;
            FirstLetter = plugin.FirstLetter;
            KeyBind = plugin.KeyBind;
            AnimalType = plugin.AnimalType;
            Color = plugin.Color;
            Speed = plugin.Speed;
            Range = plugin.Range;
            Health = plugin.Health;
            BreedingCooldown = plugin.BreedingCooldown;
            BreedingTime = plugin.BreedingTime;
            ActiveBreedingCooldown = plugin.ActiveBreedingCooldown;
            IsBirthing = plugin.IsBirthing;
        }
    }
}