using ClassLibrary.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class PluginLoaderTests
    {
        private PluginLoader _pluginLoader;
        public PluginLoaderTests()
        {
            _pluginLoader = new();
        }
        [TestMethod()]
        public void LoadPluginsTest()
        {
            var plugins = _pluginLoader.LoadPlugins();
            Assert.IsInstanceOfType(plugins, typeof(List<IPlugin>));
        }
    }
}