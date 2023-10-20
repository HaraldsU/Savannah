using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services
{
    public class InitializeService
    {
        private static int lastAssignedId = 0;
        private readonly GameService _gameService;

        public InitializeService(GameService gameService)
        {
            _gameService = gameService;
        }

        public Tuple<int, List<GridCellModel>> InitializeGame(int dimension)
        {
            var game = new GameStateModel
            {
                Id = _gameService.GetNextGameId(lastAssignedId),
                Grid = InitializeGrid(dimension),
                Turn = AnimalTypeEnums.Predator,
                CurrentTypeIndex = 0,
                Dimensions = dimension
            };
            _gameService.Games.Add(game);
            var returnData = Tuple.Create(game.Id, game.Grid);

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
