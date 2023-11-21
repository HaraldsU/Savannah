using Savanna.Commons.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Savanna.Data.Models.DB
{
    public class GameStateModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public List<GridCellModel> Grid { get; set; } = new();
        public AnimalTypeEnums Turn { get; set; }
        public int CurrentTypeIndex { get; set; }
        public int Dimensions { get; set; }
    }
}
