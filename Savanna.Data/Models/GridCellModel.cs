using Savanna.Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savanna.Data.Models
{
    public class GridCellModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CellId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public AnimalBase? Animal { get; set; } = null;
    }
}
