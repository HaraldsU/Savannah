using Savanna.Data;
using System.Reflection;

namespace Savanna.Services.PluginHandlers
{
    public class PluginLoader
    {
        public Tuple<List<IAnimal>, string> LoadPlugins()
        {
            var pluginsLists = new List<IAnimal>();
            //LoadImportedPlugins(pluginsLists);
            LoadExistingPlugins(pluginsLists);
            var validation = ValidatePlugins(pluginsLists);
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return Tuple.Create(pluginsLists, validation.Item2);
        }
        private void LoadImportedPlugins(List<IAnimal> pluginsLists)
        {
            string pluginDirectory = Environment.GetEnvironmentVariable("PLUGIN_DIRECTORY");
            // Read the dll files from the extensions folder
            var files = Directory.GetFiles(pluginDirectory, "*.dll");

            // Read the assembly from files 
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

                // Exteract all the types that implements IPlugin 
                var pluginTypes = assembly.GetTypes().Where(t => typeof(IAnimal).IsAssignableFrom(t) && !t.IsInterface).ToArray();

                foreach (var pluginType in pluginTypes)
                {
                    // Create an instance from the extracted type 
                    var pluginInstance = Activator.CreateInstance(pluginType) as IAnimal;
                    pluginsLists.Add(pluginInstance);
                }
            }
        }
        private void LoadExistingPlugins(List<IAnimal> pluginsLists)
        {
            var animalLibraryAssembly = Assembly.Load("Savanna.Data");
            var types = animalLibraryAssembly.GetTypes().Where(t => typeof(IAnimal).IsAssignableFrom(t) && !t.IsInterface);
            foreach (var type in types)
            {
                if (type != typeof(AnimalBase))
                {
                    var instance = Activator.CreateInstance(type) as IAnimal;
                    pluginsLists.Add(instance);
                }
            }
        }

        private Tuple<bool, string> ValidatePlugins(List<IAnimal> pluginsLists)
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
