using Savanna.Commons.Enums;
using Savanna.Data.Models;

namespace Savanna.Data.Interfaces
{
    public interface IAnimalType
    {
        Tuple<int, int, AnimalTargetEnums> FindTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid, int heightStart, 
                                                      int heightEnd, int widthStart, int widthEnd);
        void CalculateSteps(IAnimalProperties animal, ref int steps, DirectionEnums directionSign, GridCellModel gridItem, Tuple<int, int, AnimalTargetEnums> target,
                                            bool isXCoordinate);
        void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign, ref DirectionEnums directionYSign,
                                               Tuple<int, int, AnimalTargetEnums> target);
    }
}
