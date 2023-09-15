using AnimalLibrary.Models.Animals;

namespace Savannah;
class Program
{
    public static List<IPlugin> _plugins = new List<IPlugin>();

    static void Main(string[] args)
    {
        _plugins = PluginLoader.ReadPlugins();
        LionModel lionModel = new LionModel();
        AntelopeModel antelopeModel = new AntelopeModel();
        _plugins.Add((IPlugin)lionModel);
        _plugins.Add((IPlugin)antelopeModel);
        Console.WriteLine($"{_plugins.Count} plugin(s) found\n");

        var gameFlow = new GameFlow();
        gameFlow.Run();
    }
}