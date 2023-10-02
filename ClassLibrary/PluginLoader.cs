using AnimalLibrary.Models;
using System.Reflection;

namespace ClassLibrary
{
    public class PluginLoader
    {
        public List<IPlugin> LoadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            //// Read the dll files from the extensions folder
            //var files = Directory.GetFiles("C:\\Users\\haralds.upitis\\source\\repos\\Upitis_Savanna\\Savannah\\bin\\Debug\\net7.0\\Plugins", "*.dll");

            //// Read the assembly from files 
            //foreach (var file in files)
            //{
            //    var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

            //    // Exteract all the types that implements IPlugin 
            //    var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

            //    foreach (var pluginType in pluginTypes)
            //    {
            //        // Create an instance from the extracted type 
            //        var pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
            //        pluginsLists.Add(pluginInstance);
            //    }
            //}
            var animalLibraryAssembly = Assembly.Load("AnimalLibrary");
            var types = animalLibraryAssembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var type in types)
            {
                if (type != typeof(PluginBase))
                {
                    var instance = Activator.CreateInstance(type) as IPlugin;
                    pluginsLists.Add(instance);
                }
            }

            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));

            foreach (var plugin in pluginsLists.ToList())
            {
                var isValidated = PluginValidator.ValidatePlugin(plugin);
                if (!isValidated.Item1)
                {
                    pluginsLists.Remove(plugin);
                    PluginValidator.FailedValidationMessage(isValidated.Item2, plugin);
                }
            }

            return pluginsLists;
        }
    }
}
