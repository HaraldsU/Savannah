using ClassLibrary.Models;
using ClassLibrary.Models.Animals;

namespace ClassLibrary
{
    public class UpdateGame
    {
        public void AddAnimal(char animal, List<GridCellModel> grid)
        {
            var cellIndex = RandomGenerator.Next(grid.Count);

            if (grid[cellIndex].Animal != null)
            {
                do
                {
                    cellIndex = RandomGenerator.Next(grid.Count);
                } while (grid[cellIndex].Animal != null);
            }

            if (animal == 'A')
            {
                var antelope = new AntelopeModel
                {
                    Name = 'A'
                };
                var animalModel = new AnimalsModel
                {
                    Antelope = antelope
                };
                grid[cellIndex].Animal = animalModel;
            }
            else if (animal == 'L')
            {
                var lion = new LionModel
                {
                    Name = 'L'
                };
                var animalModel = new AnimalsModel
                {
                    Lion = lion
                };
                grid[cellIndex].Animal = animalModel;
            }
        }
        public void MoveAnimals(int dimension, List<GridCellModel> grid, bool turn)
        {
            var updates = new Dictionary<int, int>();
            var beforeAnimals = GetAnimalCount(grid);

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var coordinatesOld = coordinates;

                    if (grid[coordinates].Animal != null)
                    {
                        if (turn == true)
                        {
                            if (grid[coordinates].Animal.Lion != null)
                            {
                                var target = GetTarget(grid[coordinates].X, grid[coordinates].Y, dimension, grid[coordinates], grid);

                                if (target.Item1 == -1)
                                {
                                    var directionSigns = GetRandomDirectionSigns(dimension, grid, coordinates, updates);
                                    var directionXSign = directionSigns.Item1;
                                    var directionYSign = directionSigns.Item2;
                                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign);
                                }
                                else
                                {
                                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                                    var directionXSign = directionSigns.Item1;
                                    var directionYSign = directionSigns.Item2;
                                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
                                }
                                updates.Add(coordinatesOld, coordinates);
                            }
                        }
                        else
                        {
                            if (grid[coordinates].Animal.Antelope != null)
                            {
                                var target = GetTarget(grid[coordinates].X, grid[coordinates].Y, dimension, grid[coordinates], grid);

                                if (target.Item1 == -1)
                                {
                                    var directionSigns = GetRandomDirectionSigns(dimension, grid, coordinates, updates);
                                    var directionXSign = directionSigns.Item1;
                                    var directionYSign = directionSigns.Item2;
                                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign);
                                }
                                else
                                {
                                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                                    var directionXSign = directionSigns.Item1;
                                    var directionYSign = directionSigns.Item2;
                                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
                                }
                                updates.Add(coordinatesOld, coordinates);
                            }
                        }
                    }
                }
            }

            foreach (var update in updates)
            {
                var oldValue = grid[update.Value].Animal;
                grid[update.Value].Animal = grid[update.Key].Animal;
                if (grid[update.Value].Animal != oldValue)
                    grid[update.Key].Animal = null;
                else { }
            }

            var afterAnimals = GetAnimalCount(grid);
            if (afterAnimals != beforeAnimals)
            {
                var xx = 1;
            }
        }
        private Tuple<int, int> GetTarget(int x, int y, int dimension, GridCellModel gridItem, List<GridCellModel> grid)
        {
            dimension--;
            int range = 0;

            if (gridItem.Animal != null)
            {
                if (gridItem.Animal.Lion != null)
                    range = gridItem.Animal.Lion.Range;
                else if (gridItem.Animal.Antelope != null)
                    range = gridItem.Animal.Antelope.Range;
            }

            int widthStart = Math.Max(0, gridItem.X - range);
            int heightStart = Math.Max(0, gridItem.Y - range);
            int widthEnd = Math.Min(dimension, gridItem.X + range);
            int heightEnd = Math.Min(dimension, gridItem.Y + range);

            if (gridItem.Animal != null)
            {
                if (gridItem.Animal.Lion != null)
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            heightStart, heightEnd, widthStart, widthEnd
                                           );
                else if (gridItem.Animal.Antelope != null)
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            heightStart, heightEnd, widthStart, widthEnd
                                           );
            }
            else
                return Tuple.Create(-1, -1);

            return Tuple.Create(-1, -1);
        }
        private Tuple<int, int> GetTargetForLoop(
                                                 int dimension, GridCellModel gridItem, List<GridCellModel> grid,
                                                 int heightStart, int heightEnd, int widthStart, int widthEnd
                                                )
        {
            dimension++;
            for (int i = heightStart; i < heightEnd + 1; i++) // Height (Y)
            {
                for (int j = widthStart; j < widthEnd + 1; j++) // Width (x)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var gridItemCoordinates = ((gridItem.Y + 1) * dimension) - (dimension - gridItem.X);
                    var x = grid[coordinates];
                    if (grid[coordinates].Animal != null && coordinates != gridItemCoordinates)
                    {
                        if (gridItem.Animal.Lion != null && grid[coordinates].Animal.Antelope != null)
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y);
                        else if (gridItem.Animal.Antelope != null && grid[coordinates].Animal.Lion != null)
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y);
                    }
                }
            }
            return Tuple.Create(-1, -1);
        }
        private void MoveAnimalPosition(int dimension, List<GridCellModel> grid, ref int coordinates, char directionXSign, char directionYSign,
                                        GridCellModel? gridItem = null, Tuple<int, int>? target = null)
        {
            int steps;
            int coordinatesOld = coordinates;

            // x abscissa 
            if (grid[coordinates].Animal.Lion != null)
            {
                steps = grid[coordinates].Animal.Lion.Speed;
                if (target != null)
                {
                    if (gridItem.X + steps > target.Item1 && directionXSign == '+')
                        steps--;
                    else if (gridItem.X - steps < target.Item1 && directionXSign == '-')
                        steps--;
                }
            }
            else
                steps = grid[coordinates].Animal.Antelope.Speed;

            if (directionXSign == '-' && (coordinates - steps) >= 0 && grid[coordinates - steps].Y == grid[coordinates].Y) // Move left
                coordinates -= steps;
            else if (directionXSign == '+' && (coordinates + steps) < grid.Count - 1 && grid[coordinates + steps].Y == grid[coordinates].Y)   // Move right
                coordinates += steps;

            // y abscissa 
            if (grid[coordinatesOld].Animal.Lion != null)
            {
                steps = grid[coordinatesOld].Animal.Lion.Speed;
                if (target != null)
                {
                    if (gridItem.Y + steps > target.Item2 && directionYSign == '+')
                        steps--;
                    else if (gridItem.Y - steps < target.Item2 && directionYSign == '-')
                        steps--;
                }
            }
            else
                steps = grid[coordinatesOld].Animal.Antelope.Speed;

            if (directionYSign == '-' && (coordinates - (dimension * steps)) >= 0) // Move up
                coordinates -= dimension * steps;
            else if (directionYSign == '+' && (coordinates + (dimension * steps)) < grid.Count - 1)    // Move down
                coordinates += dimension * steps;
        }
        private Tuple<char, char> GetRandomDirectionSigns(int dimension, List<GridCellModel> grid, int coordinates, Dictionary<int, int> updates)
        {
            int count = 0;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;
            char directionXSign = '\0';
            char directionYSign = '\0';

            GenerateRandomSign(ref directionXSign, ref directionYSign);
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                do
                {
                    GenerateRandomSign(ref directionXSign, ref directionYSign);
                    coordinatesTmp = coordinates;
                    MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign); // Get new coordinates
                    count++;
                } while ((coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null
                         || updates.ContainsValue(coordinatesTmp)) && count <= 8);
                if (count >= 8)
                {
                    directionXSign = 'n';
                    directionYSign = 'n';
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        private void GenerateRandomSign(ref char directionXSign, ref char directionYSign)
        {
            int directionX = RandomGenerator.Next(3);
            if (directionX == 0) directionXSign = '-';
            else if (directionX == 1) directionXSign = '+';
            else directionXSign = 'n';

            if (directionXSign == 'n')
            {
                int directionY = RandomGenerator.Next(2);
                if (directionY == 0) directionYSign = '-';
                else directionYSign = '+';
            }
            else
            {
                int directionY = RandomGenerator.Next(3);
                if (directionY == 0) directionYSign = '-';
                else if (directionY == 1) directionYSign = '+';
                else directionYSign = 'n';

                if (directionYSign == 'n')
                {
                    directionX = RandomGenerator.Next(2);
                    if (directionX == 0) directionXSign = '-';
                    else if (directionX == 1) directionXSign = '+';
                }
            }
        }
        private Tuple<char, char> GetTargetDirectionSigns(int dimension, int coordinates, List<GridCellModel> grid, Tuple<int, int> target,
                                                          GridCellModel gridItem, Dictionary<int, int> updates)
        {
            char directionXSign;
            char directionYSign;
            int subjectX = gridItem.X;
            int subjectY = gridItem.Y;
            int targetX = target.Item1;
            int targetY = target.Item2;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;

            if (gridItem.Animal.Lion != null)
            {

                if (subjectX != targetX)
                {
                    if (subjectX > targetX)
                        directionXSign = '-';
                    else
                        directionXSign = '+';
                }
                else
                    directionXSign = 'n';
                if (subjectY != targetY)
                {
                    if (subjectY > targetY)
                        directionYSign = '-';
                    else
                        directionYSign = '+';
                }
                else
                    directionYSign = 'n';
            }
            else
            {
                if (subjectX != targetX)
                {
                    if (subjectX > targetX)
                        directionXSign = '+';
                    else
                        directionXSign = '-';
                }
                else
                    directionXSign = 'n';
                if (subjectY != targetY)
                {
                    if (subjectY > targetY)
                        directionYSign = '+';
                    else
                        directionYSign = '-';
                }
                else
                    directionYSign = 'n';
            }
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign, gridItem, target);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                if (gridTmp[coordinatesTmp].Animal != null)
                {
                    if ((gridTmp[coordinatesTmp].Animal.Antelope != null && gridTmp[coordinates].Animal.Antelope != null) ||
                        (gridTmp[coordinatesTmp].Animal.Lion != null && gridTmp[coordinates].Animal.Lion != null))
                    {
                        directionXSign = 'n';
                        directionYSign = 'n';
                    }
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        private int GetAnimalCount(List<GridCellModel> grid)
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (cell.Animal != null) count++;
            }
            return count;
        }
    }
}
