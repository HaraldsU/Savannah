using Savanna.Commons.Enums;
using Savanna.Data.Interfaces;

namespace Savanna.Data.Base
{
    public class PredatorBase : AnimalBase
    {
        private readonly int healthIncrease = 2;
        public override void AnimalEatsAnimal()
        {
            Health += healthIncrease;
        }
        public override void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign, ref DirectionEnums directionYSign)
        {
            if (subjectX > targetX)
            {
                directionXSign = DirectionEnums.NegativeDirectionSign;
            }
            else if (subjectX < targetX)
            {
                directionXSign = DirectionEnums.PositiveDirectionSign;
            }
            else
            {
                directionXSign = DirectionEnums.NoDirectionSign;
            }

            if (subjectY > targetY)
            {
                directionYSign = DirectionEnums.NegativeDirectionSign;
            }
            else if (subjectY < targetY)
            {
                directionYSign = DirectionEnums.PositiveDirectionSign;
            }
            else
            {
                directionYSign = DirectionEnums.NoDirectionSign;
            }
        }
    }
}
