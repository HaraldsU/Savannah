using ClassLibrary.Models;
using System.Reflection;

namespace ClassLibrary
{
    public class PluginLoader
    {
        public List<IPlugin> LoadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            
            var currentAssembly = Assembly.GetExecutingAssembly();
            var types = currentAssembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as IPlugin;
                pluginsLists.Add(instance);
            }
            
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return pluginsLists;
        }
    }
}
