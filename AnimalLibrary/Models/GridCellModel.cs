namespace AnimalLibrary.Models
{
    public class GridCellModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public AnimalsModel? Animal { get; set; } = null;
    }
}
