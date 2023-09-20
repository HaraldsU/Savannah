namespace AnimalLibrary.Models
{
    public interface IPlugin
    {
        string Name { get; set; }
        char FirstLetter { get; set; }
        ConsoleKey KeyBind { get; set; }
        [Range(0, 1)]
        int Type { get; set; } // 0 for predator 1 for prey
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