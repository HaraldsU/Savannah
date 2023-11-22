namespace SavannaWebAPI.Models
{
    public class RequestsModel
    {
        public int? GameId { get; set; }
        public int? SessionId { get; set; }
        public string? UserId { get; set; }
        public string? AnimalName { get; set; }
        public int? Dimensions { get; set; }
        public bool? DisplayAnimalsAsImages { get; set; }
    }
}
