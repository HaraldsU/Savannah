using AnimalLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary
{
    public static class PluginValidator
    {
        public static Tuple<bool, List<ValidationResult>> ValidatePlugin(IPlugin plugin)
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
        public static void FailedValidationMessage(List<ValidationResult> validationResults, IPlugin plugin)
        {
            Console.WriteLine("Error importing plugin: " + plugin.Name);
            foreach (var validationResult in validationResults)
            {
                Console.WriteLine(validationResult.ErrorMessage);
            }
            Environment.Exit(0);
        }
    }
}
