namespace Savannah;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var gameFlow = new GameFlow();
        gameFlow.Run();
    }
}