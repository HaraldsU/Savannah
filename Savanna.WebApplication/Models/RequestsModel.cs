namespace SavannaWebApplication.Models
{
    public class RequestsModel
    {
        public List<GridCellModelDTO>? Grid { get; set; }
        public int? Turn { get; set; }
        public int? CurrentTypeIndex { get; set; }
    }
}
