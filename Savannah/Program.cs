using AnimalLibrary.Models;
using ClassLibrary;

namespace Savannah;
class Program
{
    public static List<IPlugin> Plugins;
    private static PluginLoader _pluginLoader = new PluginLoader();
    static void Main(string[] args)
    {
        Plugins = _pluginLoader.LoadPlugins();
        var gameFlow = new GameFlow();
        gameFlow.Run();
    }
}