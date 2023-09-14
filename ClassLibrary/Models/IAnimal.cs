using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public interface IAnimal
    {
        int Speed { get; set; }
        int Range { get; set; }
        float Health { get; set; }
        int BreedingCooldown { get; set; }
        int BreedingTime { get; set; }
        int ActiveBreedingCooldown { get; set; }
        bool IsBirthing { get; set; }
    }
}
