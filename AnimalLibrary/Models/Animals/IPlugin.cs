namespace AnimalLibrary.Models.Animals
{
    public interface IPlugin
    {
        string Name { get; }
        char FirstLetter { get; }
        string Type { get; }
        string Color { get; }
        int Speed { get; }
        int Range { get; }
        float Health { get; set; }
        int BreedingCooldown { get; set; }
        int BreedingTime { get; set; }
        int ActiveBreedingCooldown { get; set; }
        bool IsBirthing { get; set; }
    }
}
