using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services
{
    public class InitializeService
    {
        public Tuple<int, List<GridCellModel>> InitializeGame(int dimension, ref GameStateModel oldGameState, int newId)
        {
            var newGameState = new GameStateModel
            {
                Id = newId,
                Grid = InitializeGrid(dimension),
                Turn = AnimalTypeEnums.Predator,
                CurrentTypeIndex = 0,
                Dimensions = dimension
            };
            oldGameState = newGameState;
            var returnData = Tuple.Create(newGameState.Id, newGameState.Grid);

            return returnData;
        }
        private List<GridCellModel> InitializeGrid(int dimension)
        {
            List<GridCellModel> grid = new();
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var cell = new GridCellModel
                    {
                        Y = i,
                        X = j
                    };
                    grid.Add(cell);
                }
            }

            return grid;
        }
    }
}
