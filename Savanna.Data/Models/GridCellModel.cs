namespace Savanna.Data.Models
{
    public class GridCellModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public IAnimal? Animal { get; set; } = null;
    }
}
