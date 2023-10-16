using Savanna.Commons.Enums;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;

namespace Savanna.Data.Base
{
    public class PreyBase : AnimalBase, IAnimalType
    {
        public override Tuple<int, int, AnimalTargetEnums> FindTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid, int heightStart,
                                            int heightEnd, int widthStart, int widthEnd)
        {
            dimension++;
            // Height (Y)
            for (int i = heightStart; i < heightEnd + 1; i++)
            {
                // Width (X)
                for (int j = widthStart; j < widthEnd + 1; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var gridItemCoordinates = ((gridItem.Y + 1) * dimension) - (dimension - gridItem.X);
                    if (grid[coordinates].Animal != null && coordinates != gridItemCoordinates)
                    {
                        // Prey fleeing predator
                        if (gridItem.Animal.AnimalType == AnimalTypeEnums.Prey && grid[coordinates].Animal.AnimalType == AnimalTypeEnums.Predator)
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetEnums.Enemy);
                        }
                        // Prey breeding prey
                        else if (gridItem.Animal.AnimalType == grid[coordinates].Animal.AnimalType &&
                                 (gridItem.Animal.ActiveBreedingCooldown == 0 || gridItem.Animal.ActiveBreedingCooldown > gridItem.Animal.BreedingCooldown) &&
                                 (grid[coordinates].Animal.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.ActiveBreedingCooldown > gridItem.Animal.BreedingCooldown) &&
                                 (gridItem.Animal.Name == grid[coordinates].Animal.Name)
                                )
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetEnums.MatingPartner);
                        }
                    }
                }
            }
            return Tuple.Create(-1, -1, AnimalTargetEnums.None);
        }
        public override void CalculateSteps(IAnimalProperties animal, ref int steps, DirectionEnums directionSign, GridCellModel gridItem, Tuple<int, int, AnimalTargetEnums> target,
                                            bool isXCoordinate)
        {
            var subjectCoordinate = isXCoordinate == true ? gridItem.X : gridItem.Y;
            var targetCoordinate = isXCoordinate == true ? target.Item1 : target.Item2;
            if (target.Item3 == AnimalTargetEnums.MatingPartner)
            {
                if (subjectCoordinate + steps >= targetCoordinate && directionSign == DirectionEnums.PositiveDirectionSign)
                {
                    steps--;
                }
                else if (subjectCoordinate - steps <= targetCoordinate && directionSign == DirectionEnums.NegativeDirectionSign)
                {
                    steps--;
                }
            }
            else
            {
                if (subjectCoordinate + steps > targetCoordinate && directionSign == DirectionEnums.PositiveDirectionSign)
                {
                    steps--;
                }
                else if (subjectCoordinate - steps < targetCoordinate && directionSign == DirectionEnums.NegativeDirectionSign)
                {
                    steps--;
                }
            }
        }
        public override void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign, ref DirectionEnums directionYSign,
                                               Tuple<int, int, AnimalTargetEnums> target)
        {
            if (subjectX != targetX)
            {
                if (subjectX > targetX)
                {
                    directionXSign = DirectionEnums.PositiveDirectionSign;
                }
                else
                {
                    directionXSign = DirectionEnums.NegativeDirectionSign;
                }
            }
            else
            {
                directionXSign = DirectionEnums.NoDirectionSign;
            }
            if (subjectY != targetY)
            {
                if (subjectY > targetY)
                {
                    directionYSign = DirectionEnums.PositiveDirectionSign;
                }
                else
                {
                    directionYSign = DirectionEnums.NegativeDirectionSign;
                }
            }
            else
            {
                directionYSign = DirectionEnums.NoDirectionSign;
            }
        }
    }
}
