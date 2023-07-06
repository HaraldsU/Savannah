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
                    var coordinates = i * dimension + j;
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
            int widthStart;
            int widthEnd;
            int heightStart;
            int heightEnd;


            int antelopeRange = 0;
            int lionRange = 0;
            int commonHeightStart = 0;
            int commonWidthStart = 0;
            int commonWidthEnd = 0;

            if (gridItem.Animal.Antelope != null)
            {
                antelopeRange = gridItem.Animal.Antelope.Range;
                commonHeightStart = gridItem.Y - (antelopeRange - ((gridItem.Y % antelopeRange)));
                commonWidthEnd = (gridItem.X + 1) + (gridItem.X % antelopeRange);
                if (gridItem.X != 0)
                    commonWidthStart = gridItem.X - (antelopeRange - (((gridItem.X + 1) % antelopeRange)));
                else
                    commonWidthStart = 0;
            }
            else if (gridItem.Animal.Lion != null)
            {
                lionRange = gridItem.Animal.Lion.Range;
                commonHeightStart = gridItem.Y - (lionRange - ((gridItem.Y % lionRange)));
                commonWidthEnd = (gridItem.X + 1) + (gridItem.X % lionRange);
                if (gridItem.X != 0)
                    commonWidthStart = gridItem.X - (lionRange - (((gridItem.X + 1) % lionRange)));
                else
                    commonWidthStart = 0;
            }

            if ((gridItem.X == 0 && gridItem.Y == 0) || (gridItem.X == (dimension - 1) && gridItem.Y == 0) || 
                (gridItem.X == 0 && gridItem.Y == (dimension - 1)) || (gridItem.X == (dimension - 1) && 
                 gridItem.Y == (dimension - 1))) // Corners
            {
                if (gridItem.X == 0 && gridItem.Y == 0) // Top left corner
                {
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            0, antelopeRange, 0, antelopeRange,
                                            0, lionRange, 0, lionRange);
                }
                else if (gridItem.X == (dimension - 1) && gridItem.Y == 0) // Top right corner
                {
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            0, antelopeRange, (dimension - 1) - antelopeRange, dimension,
                                            0, lionRange, (dimension - 1) - lionRange, dimension);
                }
                else if (gridItem.X == 0 && gridItem.Y == (dimension - 1)) // Bottom left corner
                {
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            (dimension - 1) - antelopeRange, dimension, 0, antelopeRange,
                                            (dimension - 1) - lionRange, dimension, 0, lionRange);
                }
                else // Bottom right corner
                {
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            (dimension - 1) - antelopeRange, dimension, (dimension - 1) - antelopeRange, dimension,
                                            (dimension - 1) - lionRange, dimension, (dimension - 1) - lionRange, dimension);
                }

            }
            else if ((gridItem.X == 0) || (gridItem.X == (dimension - 1)) || (gridItem.Y == 0) || (gridItem.Y == (dimension - 1)))  // Edges
            {
                if (gridItem.X == 0)  // Left edge
                {
                    if (gridItem.Animal.Antelope != null)
                    {
                        if ((gridItem.Y > antelopeRange) && ((gridItem.Y + antelopeRange) <= (dimension - 1))) // Left middle edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = gridItem.Y + antelopeRange;
                        }
                        else if ((gridItem.Y < antelopeRange) && ((gridItem.Y + antelopeRange) <= (dimension - 1)))  // Left top edge
                        {
                            heightStart = 0;
                            heightEnd = gridItem.Y + antelopeRange;
                        }
                        else  // Left bottom edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = dimension;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                heightStart, heightEnd, 0, antelopeRange,
                                                0, 0, 0, 0);
                    }
                    else if (gridItem.Animal.Lion != null)
                    {
                        if ((gridItem.Y > lionRange) && ((gridItem.Y + lionRange) <= (dimension - 1))) // Left middle edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = gridItem.Y + lionRange;
                        }
                        else if ((gridItem.Y + 1 < lionRange) && ((gridItem.Y + lionRange) <= (dimension - 1)))  // Left top edge
                        {
                            heightStart = 0;
                            heightEnd = gridItem.Y + lionRange;
                        }
                        else  // Left bottom edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = dimension;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                0, 0, 0, 0,
                                                heightStart, heightEnd, 0, lionRange);
                    }
                }
                else if (gridItem.X == (dimension - 1))  // Right edge
                {
                    if (gridItem.Animal.Antelope != null)
                    {
                        if ((gridItem.Y > antelopeRange) && ((gridItem.Y + antelopeRange) <= dimension)) // Right middle edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = gridItem.Y + antelopeRange;
                        }
                        else if ((gridItem.Y < antelopeRange) && ((gridItem.Y + antelopeRange) <= dimension))  // Right top edge
                        {
                            heightStart = 0;
                            heightEnd = gridItem.Y + antelopeRange;
                        }
                        else  // Right bottom edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = dimension;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                heightStart, heightEnd, dimension - antelopeRange, dimension,
                                                0, 0, 0, 0);
                    }
                    else if (gridItem.Animal.Lion != null)
                    {
                        if ((gridItem.Y < lionRange) && ((gridItem.Y + lionRange) <= dimension))  // Right top edge
                        {
                            heightStart = 0;
                            heightEnd = gridItem.Y + lionRange;
                        }
                        else if ((gridItem.Y > lionRange) && ((gridItem.Y + lionRange) <= dimension)) // Right middle edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = gridItem.Y + lionRange;
                        }
                        else  // Right bottom edge
                        {
                            heightStart = commonHeightStart;
                            heightEnd = dimension;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                0, 0, 0, 0,
                                                heightStart, heightEnd, dimension - lionRange, dimension);
                    }
                }
                else if (gridItem.Y == 0)  // Top edge
                {
                    if (gridItem.Animal.Antelope != null)
                    {
                        if (gridItem.X < antelopeRange) // Top left edge
                        {
                            widthStart = 0;
                            widthEnd = gridItem.X + antelopeRange;
                        }
                        else if (gridItem.X + antelopeRange > (dimension - 1))  // Top right edge
                        {
                            widthStart = commonWidthStart;
                            widthEnd = dimension;
                        }
                        else  // Top middle edge
                        {
                            widthStart = gridItem.X - antelopeRange;
                            widthEnd = gridItem.X + antelopeRange;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                0, antelopeRange, widthStart, widthEnd,
                                                0, 0, 0, 0);
                    }
                    else if (gridItem.Animal.Lion != null)
                    {
                        if ((gridItem.X + 1) < lionRange) // Top left edge
                        {
                            widthStart = 0;
                            widthEnd = gridItem.X + lionRange;
                        }
                        else if (gridItem.X + lionRange > dimension)  // Top right edge
                        {
                            widthStart = commonWidthStart;
                            widthEnd = dimension;
                        }
                        else  // Top middle edge
                        {
                            widthStart = gridItem.X - lionRange;
                            widthEnd = gridItem.X + lionRange;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                0, 0, 0, 0,
                                                0, lionRange, widthStart, widthEnd);
                    }
                }
                else  // Bottom edge
                {
                    if (gridItem.Animal.Antelope != null)
                    {
                        if (gridItem.X < antelopeRange) // Bottom left edge
                        {
                            widthStart = 0;
                            widthEnd = commonWidthEnd;
                        }
                        else if (gridItem.X + antelopeRange > (dimension - 1))  // Bottom right edge
                        {
                            widthStart = commonWidthStart;
                            widthEnd = dimension;
                        }
                        else  // Bottom middle edge
                        {
                            widthStart = gridItem.X - antelopeRange;
                            widthEnd = gridItem.X + antelopeRange;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                gridItem.Y - antelopeRange, dimension, widthStart, widthEnd,
                                                0, 0, 0, 0);
                    }
                    else if (gridItem.Animal.Lion != null)
                    {
                        if (gridItem.X < lionRange) // Bottom left edge
                        {
                            widthStart = 0;
                            widthEnd = commonWidthEnd;
                        }
                        else if (gridItem.X + lionRange > (dimension - 1))  // Bottom right edge
                        {
                            widthStart = commonWidthStart;
                            widthEnd = dimension;
                        }
                        else  // Bottom middle edge
                        {
                            widthStart = gridItem.X - lionRange;
                            widthEnd = gridItem.X + lionRange;
                        }
                        return GetTargetForLoop(dimension, gridItem, grid,
                                                0, 0, 0, 0,
                                                dimension - lionRange, dimension, widthStart, widthEnd);
                    }
                }
            }
            else // Middles
            {
                if (gridItem.Animal.Antelope != null)
                {
                    if ((gridItem.Y + antelopeRange) > (dimension - 1))  // Middle bottom
                    {
                        widthStart = 0;
                        widthEnd = commonWidthEnd;
                        heightStart = commonHeightStart;
                        heightEnd = dimension;
                    }
                    else if ((gridItem.Y - antelopeRange) < 0)  // Middle top
                    {
                        widthStart = commonWidthStart;
                        widthEnd = gridItem.X;
                        while (widthEnd < dimension) widthEnd++;
                        heightStart = 0;
                        heightEnd = gridItem.Y + antelopeRange;
                    }
                    else if ((gridItem.X - antelopeRange) < 0)  // Middle left
                    {
                        widthStart = 0;
                        widthEnd = commonWidthEnd;
                        heightStart = commonHeightStart;
                        heightEnd = gridItem.Y + antelopeRange;
                    }
                    else if ((gridItem.X + antelopeRange) > (dimension - 1))  // Middle right
                    {
                        widthStart = gridItem.X - antelopeRange;
                        widthEnd = dimension;
                        heightStart = gridItem.X - antelopeRange;
                        heightEnd = gridItem.Y + antelopeRange;
                    }
                    else  // Middle middle
                    {
                        widthStart = gridItem.X - antelopeRange;
                        widthEnd = commonWidthEnd;
                        heightStart = gridItem.Y - antelopeRange;
                        heightEnd = gridItem.Y + antelopeRange;
                    }
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            heightStart, heightEnd, widthStart, widthEnd,
                                            0, 0, 0, 0);
                }
                else if (gridItem.Animal.Lion != null)
                {

                    if ((gridItem.Y + lionRange) > (dimension - 1))  // Middle bottom
                    {
                        widthStart = 0;
                        widthEnd = commonWidthEnd;
                        heightStart = commonHeightStart;
                        heightEnd = dimension;
                    }
                    else if ((gridItem.Y - lionRange) < 0)  // Middle top
                    {
                        widthStart = commonWidthStart;
                        widthEnd = commonWidthEnd;
                        while (widthEnd < dimension) widthEnd++;
                        heightStart = 0;
                        heightEnd = gridItem.Y + lionRange;
                    }
                    else if ((gridItem.X - lionRange) < 0)  // Middle left
                    {
                        widthStart = 0;
                        widthEnd = commonWidthEnd;
                        heightStart = commonHeightStart;
                        heightEnd = gridItem.Y + lionRange;
                    }
                    else if ((gridItem.X + lionRange) > (dimension - 1))  // Middle right
                    {
                        widthStart = gridItem.X - lionRange;
                        widthEnd = dimension;
                        heightStart = gridItem.X - lionRange;
                        heightEnd = gridItem.Y + lionRange;
                    }
                    else  // Middle middle
                    {
                        widthStart = gridItem.X - lionRange;
                        widthEnd = gridItem.X + lionRange;
                        heightStart = gridItem.Y - lionRange;
                        heightEnd = gridItem.Y + lionRange;
                    }
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            0, 0, 0, 0,
                                            heightStart, heightEnd, widthStart, widthEnd);
                }
            }

            return Tuple.Create(-1, -1);
        }
        private Tuple<int, int> GetTargetForLoop(
                                                 int dimension, GridCellModel gridItem, List<GridCellModel> grid,
                                                 int heightStart1, int heightEnd1, int widthStart1, int widthEnd1,
                                                 int heightStart2, int heightEnd2, int widthStart2, int widthEnd2
                                                )
        {
            if (gridItem.Animal.Antelope != null)
            {
                for (int i = heightStart1; i < heightEnd1; i++) // Height (Y)
                {
                    for (int j = widthStart1; j < widthEnd1; j++) // Width (x)
                    {
                        var coordinates = ((i + 1) * dimension) - (j + 1);
                        var x = grid[coordinates];
                        if (grid[coordinates].Animal != null && coordinates != (((gridItem.Y + 1) * dimension) - gridItem.X) && grid[coordinates].Animal.Lion != null)
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y);
                        }
                    }
                }
                return Tuple.Create(-1, -1);
            }
            else if (gridItem.Animal.Lion != null)
            {
                for (int i = heightStart2; i < heightEnd2; i++) // Height (Y)
                {
                    for (int j = widthStart2; j < widthEnd2; j++) // Width (x)
                    {
                        var coordinates = ((i + 1) * dimension) - (j + 1);
                        var x = grid[coordinates];
                        if (grid[coordinates].Animal != null && coordinates != (((gridItem.Y + 1) * dimension) - gridItem.X) && grid[coordinates].Animal.Antelope != null)
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y);
                        }
                    }
                }
                return Tuple.Create(-1, -1);
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
            char directionXSign;
            int directionX = RandomGenerator.Next(3);
            if (directionX == 0) directionXSign = '-';
            else if (directionX == 1) directionXSign = '+';
            else directionXSign = 'n';

            char directionYSign;
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
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                do
                {
                    directionX = RandomGenerator.Next(3);
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
                    coordinatesTmp = coordinates;
                    MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign); // Get new coordinates
                    var xx = 2;
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
            if (coordinatesTmp == coordinatesOld || updates.ContainsValue(coordinatesTmp))
            {
                if (gridTmp[coordinatesTmp].Animal != null)
                {
                    if ((gridTmp[coordinatesTmp].Animal.Antelope != null && gridTmp[coordinates].Animal.Antelope != null) &&
                        gridTmp[coordinatesTmp].Animal.Lion != null && gridTmp[coordinates].Animal.Lion != null)
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
