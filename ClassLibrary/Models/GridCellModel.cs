using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Models.Animals;

namespace ClassLibrary.Models
{
    public class GridCellModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public AnimalsModel? Animal { get; set; }
    }
}
