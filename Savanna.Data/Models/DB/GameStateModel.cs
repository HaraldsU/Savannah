using Savanna.Commons.Enums;
using System.ComponentModel.DataAnnotations;

namespace Savanna.Data.Models.DB
{
    public class GameStateModel
    {
        [Key]
        public int Id { get; set; }
        public List<GridCellModel> Grid { get; set; } = new();
        public AnimalTypeEnums Turn { get; set; }
        public int CurrentTypeIndex { get; set; }
        public int Dimensions { get; set; }
    }
}
