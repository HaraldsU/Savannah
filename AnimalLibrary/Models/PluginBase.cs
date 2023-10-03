using AnimalLibrary.CustomValidations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnimalLibrary.Models
{
    public abstract class PluginBase : IPlugin
    {
        [JsonConstructor]
        public PluginBase()
        {
        }
        [Required]
        public string Name { get; set; }

        [Required]
        public char FirstLetter { get; set; }

        [Required]
        [NotNoName]
        public ConsoleKey KeyBind { get; set; }

        [Required]
        public bool IsPrey { get; set; }

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
        public float Health { get; set; } = 6f;

        [Required]
        [Range(1, 10, ErrorMessage = "BreedingCooldown must be between 1 and 10")]
        public int BreedingCooldown { get; set; } = 4;

        [Required]
        [Range(1, 5, ErrorMessage = "BreedingTime must be between 1 and 5")]
        public int BreedingTime { get; set; } = 2;

        [Required]
        public int ActiveBreedingCooldown { get; set; } = 0;

        [Required]
        public bool IsBirthing { get; set; } = false;
        public abstract IPlugin CreateNewAnimal();
    }
}
