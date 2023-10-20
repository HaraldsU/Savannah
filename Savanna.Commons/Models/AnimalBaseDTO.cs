namespace Savanna.Commons.Models
{
    public class AnimalBaseDTO
    {
        public string? Name { get; set; }
        public char FirstLetter { get; set; }
        public ConsoleKey KeyBind { get; set; }
        public string? Color { get; set; }
        public AnimalBaseDTO()
        {

        }
    }
}