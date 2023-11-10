using Savanna.Data.Models.DB;

namespace Savanna.Data.Models
{
    public class CurrentGameModel
    {
        public GameStateModel Game { get; set; } = new();
        public int SessionId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
