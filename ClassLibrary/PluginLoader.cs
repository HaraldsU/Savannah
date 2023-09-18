using AnimalLibrary.Models.Animals;
using System.Reflection;

namespace Savannah
{
    public class PluginLoader
    {
        public List<IPlugin> LoadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            // 1 - Read the dll files from the extensions folder
            var files = Directory.GetFiles("C:\\Users\\haralds.upitis\\source\\repos\\Upitis_Savanna\\Savannah\\bin\\Debug\\net7.0\\Plugins", "*.dll");

            // 2 - Read the assembly from files 
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

                // 3 - Exteract all the types that implements IPlugin 
                var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

                foreach (var pluginType in pluginTypes)
                {
                    // 4 - Create an instance from the extracted type 
                    var pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
                    pluginsLists.Add(pluginInstance);
                }
            }
            LionModel lionModel = new();
            AntelopeModel antelopeModel = new();
            pluginsLists.Add(lionModel);
            pluginsLists.Add(antelopeModel);
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return pluginsLists;
        }
    }
}
