using Savanna.Commons.Enums;
using Savanna.Data.Models;
using Savanna.Data.Models.DB;

namespace Savanna.Services
{
    public class InitializeService
    {
        public static List<GameStateModel> Games = new();
        private readonly SavannaContext _dbContext;

        public InitializeService(SavannaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Tuple<int, List<GridCellModel>> InitializeGame(int dimension)
        {
            var game = new GameStateModel
            {
                Id = GetNextGameId(),
                Grid = InitializeGrid(dimension),
                Turn = AnimalTypeEnums.Predator,
                CurrentTypeIndex = 0,
                Dimensions = dimension
            };
            Games.Add(game);
            var returnData = Tuple.Create(game.Id, game.Grid);

            return returnData;
        }
        private int GetNextGameId()
        {
            var lastId = (from gameState in _dbContext.GameState
                           orderby gameState.Id descending
                           select gameState.Id).FirstOrDefault();
            return lastId + 1;
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
