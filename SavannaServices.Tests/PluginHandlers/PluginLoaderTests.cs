using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibraryTests.PluginHandlers
{
    [TestClass()]
    public class PluginLoaderTests
    {
        private GameService _gameService = new();
        [TestMethod()]
        public void LoadPluginsTest()
        {
            Assert.IsInstanceOfType(_gameService.Animals, typeof(List<IAnimal>));
        }
    }
}