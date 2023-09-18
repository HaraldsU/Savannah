using AnimalLibrary.Models.Animals;

namespace Savannah;
class Program
{
    public static List<IPlugin> _plugins = new List<IPlugin>();
    public static PluginLoader _pluginLoader = new PluginLoader();
    static void Main(string[] args)
    {
        _plugins = _pluginLoader.LoadPlugins();
        Console.WriteLine($"{_plugins.Count} plugin(s) found\n");

        var gameFlow = new GameFlow();
        gameFlow.Run();
    }
}