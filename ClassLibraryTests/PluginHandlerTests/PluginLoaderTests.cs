using AnimalLibrary.Models;
using ClassLibrary;
using ClassLibrary.PluginHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibraryTests.PluginHandlers
{
    [TestClass()]
    public class PluginLoaderTests
    {
        [TestMethod()]
        public void LoadPluginsTest()
        {
            Assert.IsInstanceOfType(AnimalListSingleton.Instance.GetAnimalList(), typeof(List<IPlugin>));
        }
    }
}