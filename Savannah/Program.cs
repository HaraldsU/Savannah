using AnimalLibrary.Models;

namespace Savannah;
class Program
{
    static List<IPlugin> _plugins = null;
    static void Main(string[] args)
    {
        _plugins = PluginLoader.ReadExtensions();
        Console.WriteLine($"{_plugins.Count} plugin(s) found");
        // Print 
        foreach (var plugin in _plugins)
        {

        }

        //var gameFlow = new GameFlow();
        //gameFlow.Run();
    }
}