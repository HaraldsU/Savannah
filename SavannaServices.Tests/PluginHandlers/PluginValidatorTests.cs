using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Data.Models.Animals;
using Savanna.Services.PluginHandlers;

namespace ClassLibrary.PluginHandlers.Tests
{
    [TestClass()]
    public class PluginValidatorTests
    {
        [TestMethod()]
        public void ValidatePluginTest()
        {
            // Arrange
            var pluginValidationFailed = new AntelopeModel
            {
                Color = "WrongColor"
            };
            var pluginValidationValid = new AntelopeModel();

            // Act
            var validationFailed = PluginValidator.ValidatePlugin(pluginValidationFailed);
            var validationValid = PluginValidator.ValidatePlugin(pluginValidationValid);

            // Assert
            Assert.AreEqual(false, validationFailed.Item1);
            Assert.AreEqual(true, validationValid.Item1);
        }

        [TestMethod()]
        public void FailedValidationMessageTest()
        {
            // Arrange
            var pluginValidationFailed = new AntelopeModel
            {
                Color = "WrongColor"
            };
            var validationFailed = PluginValidator.ValidatePlugin(pluginValidationFailed);

            // Act
            var validationMessage = PluginValidator.FailedValidationMessage(validationFailed.Item2, pluginValidationFailed);

            // Assert
            Assert.IsNotNull(validationMessage);
        }
    }
}