using Savanna.Commons.Enums;
using Savanna.Data.Interfaces;

namespace Savanna.Data.Base
{
    public class PreyBase : AnimalBase
    {
        public override void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign,
                                               ref DirectionEnums directionYSign)
        {
            if (subjectX > targetX)
            {
                directionXSign = DirectionEnums.PositiveDirectionSign;
            }
            else if (subjectX < targetX)
            {
                directionXSign = DirectionEnums.NegativeDirectionSign;
            }
            else
            {
                directionXSign = DirectionEnums.NoDirectionSign;
            }

            if (subjectY > targetY)
            {
                directionYSign = DirectionEnums.PositiveDirectionSign;
            }
            else if (subjectY < targetY)
            {
                directionYSign = DirectionEnums.NegativeDirectionSign;
            }
            else
            {
                directionYSign = DirectionEnums.NoDirectionSign;
            }
        }
    }
}
