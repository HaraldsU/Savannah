using System.ComponentModel.DataAnnotations;

namespace Savanna.Data.CustomValidations
{
    public class NotNoNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is ConsoleKey key && key == ConsoleKey.NoName)
            {
                return new ValidationResult("KeyBind cannot be ConsoleKey.NoName");
            }
            return ValidationResult.Success;
        }
    }
}
