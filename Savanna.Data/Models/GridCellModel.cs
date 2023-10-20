using Microsoft.EntityFrameworkCore;
using Savanna.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savanna.Data.Models
{
    [Keyless]
    public class GridCellModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public AnimalBase? Animal { get; set; } = null;
    }
}
