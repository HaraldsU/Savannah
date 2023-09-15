using AnimalLibrary.Models.Animals;
using System.Reflection;

namespace Savannah
{
    public class PluginLoader
    {
        public static List<IPlugin> ReadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            // 1 - Read the dll files from the extensions folder
            var files = Directory.GetFiles("Plugins", "*.dll");

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

            return pluginsLists;
        }
    }
}
