using Savanna.Commons.Constants;
using Savanna.Data.Base;
using Savanna.Data.Interfaces;
using Savanna.Services.Constants;
using System.Reflection;

namespace Savanna.Services.PluginHandlers
{
    public class PluginLoader
    {
        public Tuple<List<IAnimalProperties>, string> LoadPlugins()
        {
            var pluginsLists = new List<IAnimalProperties>();
            //LoadImportedPlugins(pluginsLists);
            LoadExistingPlugins(pluginsLists);
            var validation = ValidatePlugins(pluginsLists);
            pluginsLists.Sort((plugin1, plugin2) => plugin1.FirstLetter.CompareTo(plugin2.FirstLetter));
            return Tuple.Create(pluginsLists, validation.Item2);
        }
        private void LoadImportedPlugins(List<IAnimalProperties> pluginsLists)
        {
            string pluginDirectory = Environment.GetEnvironmentVariable(EnvironmentVariableConstants.PluginDirectory);
            // Read the dll files from the extensions folder
            var files = Directory.GetFiles(pluginDirectory, "*.dll");

            // Read the assembly from files 
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

                // Exteract all the types that implements IPlugin 
                var pluginTypes = assembly.GetTypes().Where(t => typeof(IAnimalProperties).IsAssignableFrom(t) && !t.IsInterface).ToArray();

                foreach (var pluginType in pluginTypes)
                {
                    // Create an instance from the extracted type 
                    var pluginInstance = Activator.CreateInstance(pluginType) as IAnimalProperties;
                    pluginsLists.Add(pluginInstance);
                }
            }
        }
        private void LoadExistingPlugins(List<IAnimalProperties> pluginsLists)
        {
            var animalLibraryAssembly = Assembly.Load(AssemblyConstants.SavannaData);

            var baseClasses = new List<Type>
            {
                typeof(PreyBase),
                typeof(PredatorBase)
            };

            var types = animalLibraryAssembly.GetTypes()
                .Where(t => typeof(IAnimalProperties).IsAssignableFrom(t) && !t.IsInterface)
                .Where(t => baseClasses.Any(baseType => t.IsSubclassOf(baseType)));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as IAnimalProperties;
                pluginsLists.Add(instance);
            }
        }


        private Tuple<bool, string> ValidatePlugins(List<IAnimalProperties> pluginsLists)
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
