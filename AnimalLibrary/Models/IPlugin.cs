namespace AnimalLibrary.Models
{
    public interface IPlugin
    {
        char Name { get; }
        int Speed { get; }
        int Range { get; }
        float Health { get; set; }
        int BreedingCooldown { get; set; }
        int BreedingTime { get; set; }
        int ActiveBreedingCooldown { get; set; }
        bool IsBirthing { get; set; }
    }
}
