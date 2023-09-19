<<<<<<<< HEAD:AnimalLibrary/Models/Animals/IPlugin.cs
﻿namespace AnimalLibrary.Models.Animals
========
﻿namespace ClassLibrary.Models
>>>>>>>> Iter-2:ClassLibrary/Models/IPlugin.cs
{
    public interface IPlugin
    {
        string Name { get; set; }
        char FirstLetter { get; set; }
        string Type { get; set; }
        string Color { get; set; }
        int Speed { get; set; }
        int Range { get; set; }
        float Health { get; set; }
        int BreedingCooldown { get; set; }
        int BreedingTime { get; set; }
        int ActiveBreedingCooldown { get; set; }
        bool IsBirthing { get; set; }
        IPlugin CreateNewAnimal();
    }
}