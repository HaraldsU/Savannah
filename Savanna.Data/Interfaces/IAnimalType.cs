using Savanna.Commons.Enums;

namespace Savanna.Data.Interfaces
{
    public interface IAnimalType
    {
        /// <summary>
        /// Determines the movement direction of the animal depending on the position of the target.
        /// </summary>
        /// <param name="subjectX"></param>
        /// <param name="subjectY"></param>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <param name="directionXSign"></param>
        /// <param name="directionYSign"></param>
        void SetDirectionSigns(int subjectX, int subjectY, int targetX, int targetY, ref DirectionEnums directionXSign, ref DirectionEnums directionYSign);
    }
}
