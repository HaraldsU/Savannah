using ClassLibrary.Models.Animals;
using ClassLibrary;

namespace Savannah;
class Program
{
    public static List<IPlugin> Plugins;
    public static PluginLoader _pluginLoader = new PluginLoader();
    static void Main(string[] args)
    {
        Plugins = _pluginLoader.LoadPlugins();
        Console.WriteLine($"{Plugins.Count} plugin(s) found\n");

        var gameFlow = new GameFlow();
        gameFlow.Run();
    }
}