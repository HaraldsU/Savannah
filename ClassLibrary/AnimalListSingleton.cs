using AnimalLibrary.Models;
using ClassLibrary.PluginHandlers;

namespace ClassLibrary
{
    public sealed class AnimalListSingleton
    {
        private static AnimalListSingleton instance = null;
        private static readonly object padlock = new();

        private PluginLoader _pluginLoader = new();
        private readonly Tuple<List<IPlugin>, string> animalList;

        private AnimalListSingleton()
        {
            animalList = _pluginLoader.LoadPlugins();
        }

        public static AnimalListSingleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AnimalListSingleton();
                    }
                    return instance;
                }
            }
        }
        public List<IPlugin> GetAnimalList()
        {
            return animalList.Item1;
        }
        public string GetAnimalListValidationErrors()
        {
            return animalList.Item2;
        }
    }
}