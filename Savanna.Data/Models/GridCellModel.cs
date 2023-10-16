using Savanna.Data.Base;

namespace Savanna.Data.Models
{
    public class GridCellModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public AnimalBase? Animal { get; set; } = null;
    }
}
