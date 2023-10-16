using Savanna.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Savanna.Services.PluginHandlers
{
    public static class PluginValidator
    {
        public static Tuple<bool, List<ValidationResult>> ValidatePlugin(IAnimalProperties plugin)
        {
            var validationContext = new ValidationContext(plugin);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(plugin, validationContext, validationResults, true);

            if (!isValid)
            {
                return Tuple.Create(false, validationResults);
            }
            return Tuple.Create(true, validationResults);
        }
        public static StringBuilder FailedValidationMessage(List<ValidationResult> validationResults, IAnimalProperties plugin)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Error importing plugin: " + plugin.Name);
            foreach (var validationResult in validationResults)
            {
                stringBuilder.AppendLine(validationResult.ErrorMessage);
            }
            return stringBuilder;
        }
    }
}
