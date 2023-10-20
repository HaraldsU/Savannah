using Savanna.Commons.Models;

namespace SavannaWebApplication.Models
{
    public class ResponseModel
    {
        public int GameId { get; set; }
        public List<GridCellModelDTO>? Grid { get; set; }
    }
}
