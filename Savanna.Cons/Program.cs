namespace Savanna.Cons;
class Program
{
    static void Main(string[] args)
    {
        HttpClient httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5266"),
            DefaultRequestHeaders =
            {
                { "Accept", "application/json" }
            }
        };

        var gameFlow = new GameFlow();
        var gameFlowTask = gameFlow.Run(httpClient);
        gameFlowTask.Wait();
    }
}