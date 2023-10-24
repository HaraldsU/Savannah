using Savanna.Commons.Models;

namespace Savanna.Cons.Models
{
    public class ResponseModel
    {
        public int GameId { get; set; }
        public List<GridCellModelDTO>? Grid { get; set; } = new();
    }
}
