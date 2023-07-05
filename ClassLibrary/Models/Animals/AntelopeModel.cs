using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Animals
{
    public class AntelopeModel
    {
        public char Name { get; set; } = '\0';
        public readonly int Speed = 1;
        public readonly int Range = 2;
    }
}
