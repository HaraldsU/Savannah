using AnimalLibrary.Models;
using System.Reflection;

namespace ClassLibrary.PluginHandlers
{
    public class PluginLoader
    {
        public Tuple<List<IPlugin>, string> LoadPlugins()
        {
            var pluginsLists = new List<IPlugin>();
            LoadImportedPlugins(pluginsLists);
            LoadExistingPlugins(pluginsLists);
            var validation = ValidatePlugins(pluginsLists);
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return Tuple.Create(pluginsLists, validation.Item2);
        }
        private void LoadImportedPlugins(List<IPlugin> pluginsLists)
        {
            string pluginDirectory = Environment.GetEnvironmentVariable("PLUGIN_DIRECTORY");
            // Read the dll files from the extensions folder
            var files = Directory.GetFiles(pluginDirectory, "*.dll");

            //// Read the assembly from files 
            //foreach (var file in files)
            //{
            //    var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

            //    // Exteract all the types that implements IPlugin 
            //    var pluginTypes = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface).ToArray();

                foreach (var pluginType in pluginTypes)
                {
                    // Create an instance from the extracted type 
                    var pluginInstance = Activator.CreateInstance(pluginType) as IPlugin;
                    pluginsLists.Add(pluginInstance);
                }
            }
        }
        private void LoadExistingPlugins(List<IPlugin> pluginsLists)
        {
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
        }

        private Tuple<bool, string> ValidatePlugins(List<IPlugin> pluginsLists)
        {
            foreach (var plugin in pluginsLists.ToList())
            {
                var isValidated = PluginValidator.ValidatePlugin(plugin);
                if (!isValidated.Item1)
                {
                    pluginsLists.Remove(plugin);
                    string errorMessage = PluginValidator.FailedValidationMessage(isValidated.Item2, plugin).ToString();
                    return Tuple.Create(false, errorMessage);
                }
            }
            return Tuple.Create(true, string.Empty);
        }
    }
}
