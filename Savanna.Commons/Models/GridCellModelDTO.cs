namespace Savanna.Commons.Models
{
    public class GridCellModelDTO
    {
        public int? X { get; set; }
        public int? Y { get; set; }
        public AnimalBaseDTO? Animal { get; set; } = null;
    }
}
