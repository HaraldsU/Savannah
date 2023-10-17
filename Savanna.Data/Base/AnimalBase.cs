using Savanna.Commons.Enums;
using Savanna.Data.CustomValidations;
using Savanna.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Savanna.Data.Base
{
    public class AnimalBase : IAnimalProperties, IAnimalType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public char FirstLetter { get; set; }

        [Required]
        [NotNoName]
        public ConsoleKey KeyBind { get; set; }

        [Required]
        public AnimalTypeEnums AnimalType { get; set; }

        [Required]
        [AllowedColorsAtrribute("Yellow", "Dark_yellow", "Gray", "Dark_gray",
                        "Red", "Dark_red", "Blue", "Dark_blue",
                        "Green", "Dark_green", "Magenta", "Dark_magenta",
                        "Cyan", "Dark_cyan", "White"
                        )]
        public string Color { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Speed must be between 1 and 5")]
        public int Speed { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Range must be between 1 and 5")]
        public int Range { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Health must be between 1 and 10")]
        public float Health { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "BreedingCooldown must be between 1 and 10")]
        public int BreedingCooldown { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "BreedingTime must be between 1 and 5")]
        public int BreedingTime { get; set; } = 2;

        [Required]
        public int ActiveBreedingCooldown { get; set; } = 0;

        [Required]
        public bool IsBirthing { get; set; } = false;
        public virtual void AnimalEatsAnimal() { }
        public virtual void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign, 
                                              ref DirectionEnums directionYSign) { }
        public virtual AnimalBase CreateNewAnimal()
        {
            return (AnimalBase)MemberwiseClone();
        }
    }
}
