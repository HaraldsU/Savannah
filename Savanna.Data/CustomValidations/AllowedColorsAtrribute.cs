using System.ComponentModel.DataAnnotations;

namespace Savanna.Data.CustomValidations
{
    public class AllowedColorsAtrribute : ValidationAttribute
    {
        private readonly string[] _allowedColors;
        public AllowedColorsAtrribute(params string[] allowedColors)
        {
            _allowedColors = allowedColors;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string color)
            {
                if (_allowedColors.Contains(color, StringComparer.OrdinalIgnoreCase))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"Color must be one of: {string.Join(",", _allowedColors)}");
        }
    }
}
