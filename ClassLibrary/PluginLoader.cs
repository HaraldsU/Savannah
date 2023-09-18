using ClassLibrary.Models.Animals;

namespace ClassLibrary
{
    public class PluginLoader
    {
        public List<IPlugin> LoadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            LionModel lionModel = new();
            AntelopeModel antelopeModel = new();
            pluginsLists.Add(lionModel);
            pluginsLists.Add(antelopeModel);
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return pluginsLists;
        }
    }
}
