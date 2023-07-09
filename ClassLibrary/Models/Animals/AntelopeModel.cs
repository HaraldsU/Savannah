using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models.Animals
{
    public class AntelopeModel
    {
        public readonly int Speed = 1;
        public readonly int Range = 2;
        public readonly int BreedingCooldown = 5;
        public int BreedingTime = 2;
        public int ActiveBreedingCooldown = 0;
        public bool IsBirthing = false;
    }
}
