using ClassLibrary;
using ClassLibrary.Models;
using ClassLibrary.Models.Animals;
using ClassLibrary.Constants;

namespace ClassLibrary
{
    public class UpdateGame
    {
        // Adds a new antelope or lion depending on input
        public void AddAnimal(char animal, List<GridCellModel> grid, bool isChild, Dictionary<int, int>? updates = null)
        {
            ClearGridAnimals(grid);
            var cellIndex = RandomGenerator.Next(grid.Count);
            var animalCount = GetAnimalCount(grid);

            if (animalCount < grid.Count)
            {
                if (updates == null)
                {
                    if (grid[cellIndex].Animal != null)
                    {
                        do
                        {
                            cellIndex = RandomGenerator.Next(grid.Count);
                        } while (grid[cellIndex].Animal != null);
                    }
                }
                else
                {
                    if ((grid[cellIndex].Animal != null) || updates.ContainsValue(cellIndex))
                    {
                        do
                        {
                            cellIndex = RandomGenerator.Next(grid.Count);
                            if (grid[cellIndex].Animal == null && isChild == true)
                            {
                                break;
                            }
                        } while ((grid[cellIndex].Animal != null) || updates.ContainsValue(cellIndex));
                    }
                }

                if (animal == 'A')
                {
                    var antelope = new AntelopeModel();
                    if (isChild)
                        antelope.ActiveBreedingCooldown = antelope.BreedingCooldown;

                    var animalModel = new AnimalsModel
                    {
                        Antelope = antelope
                    };
                    grid[cellIndex].Animal = animalModel;
                }
                else if (animal == 'L')
                {
                    var lion = new LionModel();
                    if (isChild)
                        lion.ActiveBreedingCooldown = lion.BreedingCooldown;
                    var animalModel = new AnimalsModel
                    {
                        Lion = lion
                    };
                    grid[cellIndex].Animal = animalModel;
                }
            }
        }
        // Sets the new animals positions and clears the old ones
        public void MoveAnimals(int dimension, List<GridCellModel> grid, bool turn)
        {
            var updates = new Dictionary<int, int>();
            GetAnimalsNewPositions(dimension, grid, turn, updates);

            foreach (var update in updates)
            {
                var oldValue = grid[update.Value].Animal;
                // Moving of the animal
                grid[update.Value].Animal = grid[update.Key].Animal;
                if (grid[update.Key].Animal != null)
                {
                    DeleteAnimalNoHealth(grid[update.Key].Animal);
                }
                // If animal moved, empty the previous cell 
                if (grid[update.Value].Animal != oldValue)
                {
                    grid[update.Key].Animal = null;
                }
            }
        }
        // Gets the animals new positions
        private void GetAnimalsNewPositions(int dimension, List<GridCellModel> grid, bool turn, Dictionary<int, int> updates)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var coordinatesOld = coordinates;

                    if (grid[coordinates].Animal != null)
                    {
                        // Lion moving
                        if (turn == true)
                        {
                            if (grid[coordinates].Animal.Lion != null)
                            {
                                GetAnimalNewPosition(dimension, grid, coordinates, coordinatesOld, 'L', updates);
                            }
                        }
                        // Antelope moving
                        else
                        {
                            if (grid[coordinates].Animal.Antelope != null)
                            {
                                GetAnimalNewPosition(dimension, grid, coordinates, coordinatesOld, 'A', updates);
                            }
                        }
                    }
                }
            }
        }
        // Gets one animal new position
        private void GetAnimalNewPosition(int dimension, List<GridCellModel> grid, int coordinates, int coordinatesOld, char animal, Dictionary<int, int> updates)
        {
            var animalCount = GetAnimalCount(grid);
            var target = GetTarget(dimension, grid[coordinates], grid);
            var currentAnimal = animal == 'A' ? (IAnimal)grid[coordinates].Animal.Antelope : grid[coordinates].Animal.Lion;

            if (currentAnimal != null)
            {
                if (currentAnimal.ActiveBreedingCooldown != 0) // Reset cooldown
                {
                    currentAnimal.ActiveBreedingCooldown--;
                    if (currentAnimal.ActiveBreedingCooldown == currentAnimal.BreedingCooldown)
                        currentAnimal.IsBirthing = false;
                }

                // No target animals in range
                if (target.Item1 == -1)
                {
                    var directionSigns = GetRandomDirectionSigns(dimension, grid, coordinates, updates);
                    var directionXSign = directionSigns.Item1;
                    var directionYSign = directionSigns.Item2;
                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign);
                }
                else if (target.Item3 == AnimalTargetConstants.breed) // Other animal of same type in range
                {
                    var targetIndex = ((target.Item2 + 1) * dimension) - (dimension - target.Item1);
                    var targetAnimal = animal == 'A' ? (IAnimal)grid[targetIndex].Animal.Antelope : (IAnimal)grid[targetIndex].Animal.Lion;
                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                    var directionXSign = directionSigns.Item1;
                    var directionYSign = directionSigns.Item2;
                    var x = grid[coordinates];
                    // Animal breeds animal
                    if (directionXSign == DirectionConstants.noDirectionSign && directionYSign == DirectionConstants.noDirectionSign
                        && (currentAnimal.ActiveBreedingCooldown == 0 && targetAnimal.ActiveBreedingCooldown == 0))
                    {
                        currentAnimal.ActiveBreedingCooldown = currentAnimal.BreedingTime + currentAnimal.BreedingCooldown;
                        currentAnimal.IsBirthing = true;
                        targetAnimal.ActiveBreedingCooldown = targetAnimal.BreedingTime + targetAnimal.BreedingCooldown;
                    }
                    else if (currentAnimal.ActiveBreedingCooldown == currentAnimal.BreedingCooldown + 1 && currentAnimal.IsBirthing == true)
                    {
                        if ((updates.Count + animalCount) >= grid.Count)
                        {
                            var updatesElement = updates.Where(a => a.Key != a.Value).FirstOrDefault();
                            updates.Remove(updatesElement.Key);
                            updates.Add(updatesElement.Key, updatesElement.Key);
                        }
                        AddAnimal(animal, grid, true, updates);
                        directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                        directionXSign = directionSigns.Item1;
                        directionXSign = directionSigns.Item2;
                    }
                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
                }
                else // Target animal in range
                {
                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                    var directionXSign = directionSigns.Item1;
                    var directionYSign = directionSigns.Item2;
                    MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
                }
                if (!updates.ContainsKey(coordinatesOld))
                {
                    updates.Add(coordinatesOld, coordinates);
                }
                RemoveAnimalHealth(currentAnimal);
            }
        }
        // Remove animal health
        private void RemoveAnimalHealth(IAnimal currentAnimal)
        {
            currentAnimal.Health -= .5f;
        }
        private void DeleteAnimalNoHealth(AnimalsModel keyAnimal)
        {
            if (keyAnimal.Lion != null)
            {
                if (keyAnimal.Lion.Health == 0)
                {
                    keyAnimal.Lion = null;
                }
            }
            else if (keyAnimal.Antelope != null)
            {
                if (keyAnimal.Antelope.Health == 0)
                {
                    keyAnimal.Antelope = null;
                }
            }
        }
        // Calls "GetTargetForLoop" with the appropriate variables or returns Tuple(-1, -1, string.Empty)
        private Tuple<int, int, string> GetTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid)
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
                return Tuple.Create(-1, -1, string.Empty);

            return Tuple.Create(-1, -1, string.Empty);
        }
        // Finds the "Target"
        private Tuple<int, int, string> GetTargetForLoop(
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
                        if (gridItem.Animal.Lion != null && grid[coordinates].Animal.Antelope != null) // Lion catching antelope
                        {
                            gridItem.Animal.Lion.Health += 2;
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetConstants.enemy);
                        }
                        else if (gridItem.Animal.Antelope != null && grid[coordinates].Animal.Lion != null) // Antelope fleeing lion
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetConstants.enemy);
                        else if (gridItem.Animal.Lion != null // Lion breeding lion
                                 && grid[coordinates].Animal.Lion != null
                                 && (gridItem.Animal.Lion.ActiveBreedingCooldown == 0 || gridItem.Animal.Lion.ActiveBreedingCooldown > gridItem.Animal.Lion.BreedingCooldown)
                                 && (grid[coordinates].Animal.Lion.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Lion.ActiveBreedingCooldown > gridItem.Animal.Lion.BreedingCooldown)
                                )
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetConstants.breed);
                        else if (gridItem.Animal.Antelope != null  // Antelope breeding antelope
                                 && grid[coordinates].Animal.Antelope != null
                                 && (gridItem.Animal.Antelope.ActiveBreedingCooldown == 0 || gridItem.Animal.Antelope.ActiveBreedingCooldown > gridItem.Animal.Antelope.BreedingCooldown)
                                 && (grid[coordinates].Animal.Antelope.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Antelope.ActiveBreedingCooldown > gridItem.Animal.Antelope.BreedingCooldown)
                                )
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetConstants.breed);
                    }
                }
            }
            return Tuple.Create(-1, -1, string.Empty);
        }
        // Gets the new coordinates
        private void MoveAnimalPosition(int dimension, List<GridCellModel> grid, ref int coordinates, char directionXSign, char directionYSign,
                                        GridCellModel? gridItem = null, Tuple<int, int, string>? target = null)
        {
            int steps;
            int coordinatesOld = coordinates;

            // x abscissa 
            // Get steps
            if (grid[coordinates].Animal.Lion != null)
            {
                steps = grid[coordinates].Animal.Lion.Speed;
                if (target != null)
                {
                    if (gridItem.X + steps > target.Item1 && directionXSign == DirectionConstants.positiveDirectionSign)
                        steps--;
                    else if (gridItem.X - steps < target.Item1 && directionXSign == DirectionConstants.negativeDirectionSign)
                        steps--;
                }
            }
            else
                steps = grid[coordinates].Animal.Antelope.Speed;

            if (directionXSign == DirectionConstants.negativeDirectionSign && (coordinates - steps) >= 0 && grid[coordinates - steps].Y == grid[coordinates].Y) // Move left
                coordinates -= steps;
            else if (directionXSign == DirectionConstants.positiveDirectionSign && (coordinates + steps) <= grid.Count - 1 && grid[coordinates + steps].Y == grid[coordinates].Y)   // Move right
                coordinates += steps;

            // y abscissa 
            // Get steps
            if (grid[coordinatesOld].Animal.Lion != null)
            {
                steps = grid[coordinatesOld].Animal.Lion.Speed;
                // target is x, y, action
                if (target != null)
                {
                    if (gridItem.Y + steps > target.Item2 && directionYSign == DirectionConstants.positiveDirectionSign)
                        steps--;
                    else if (gridItem.Y - steps < target.Item2 && directionYSign == DirectionConstants.negativeDirectionSign)
                        steps--;
                }
            }
            else
                steps = grid[coordinatesOld].Animal.Antelope.Speed;

            if (directionYSign == DirectionConstants.negativeDirectionSign && (coordinates - (dimension * steps)) >= 0) // Move up
                coordinates -= dimension * steps;
            else if (directionYSign == DirectionConstants.positiveDirectionSign && (coordinates + (dimension * steps)) <= grid.Count - 1)    // Move down
                coordinates += dimension * steps;
        }
        // Generates a random directional sign: "-", "+" or 'n' (no sign). Defaults as 'n' if fails after 8 tries
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
                    directionXSign = DirectionConstants.noDirectionSign;
                    directionYSign = DirectionConstants.noDirectionSign;
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        // Generates a random sign: "-", "+" or 'n' (no sign)
        private void GenerateRandomSign(ref char directionXSign, ref char directionYSign)
        {
            int directionX = RandomGenerator.Next(3);
            if (directionX == 0) directionXSign = DirectionConstants.negativeDirectionSign;
            else if (directionX == 1) directionXSign = DirectionConstants.positiveDirectionSign;
            else directionXSign = DirectionConstants.noDirectionSign;

            if (directionXSign == DirectionConstants.noDirectionSign)
            {
                int directionY = RandomGenerator.Next(2);
                if (directionY == 0) directionYSign = DirectionConstants.negativeDirectionSign;
                else directionYSign = DirectionConstants.positiveDirectionSign;
            }
            else
            {
                int directionY = RandomGenerator.Next(3);
                if (directionY == 0) directionYSign = DirectionConstants.negativeDirectionSign;
                else if (directionY == 1) directionYSign = DirectionConstants.positiveDirectionSign;
                else directionYSign = DirectionConstants.noDirectionSign;

                if (directionYSign == DirectionConstants.noDirectionSign)
                {
                    directionX = RandomGenerator.Next(2);
                    if (directionX == 0) directionXSign = DirectionConstants.negativeDirectionSign;
                    else if (directionX == 1) directionXSign = DirectionConstants.positiveDirectionSign;
                }
            }
        }
        // Generates a directional sign depending on the direction of the target: "-", "+" or DirectionConstants.noDirectionSign (no sign)
        private Tuple<char, char> GetTargetDirectionSigns(int dimension, int coordinates, List<GridCellModel> grid, Tuple<int, int, string> target,
                                                          GridCellModel gridItem, Dictionary<int, int> updates)
        {
            char directionXSign = DirectionConstants.noDirectionSign;
            char directionYSign = DirectionConstants.noDirectionSign;
            int subjectX = gridItem.X;
            int subjectY = gridItem.Y;
            int targetX = target.Item1;
            int targetY = target.Item2;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;

            if (gridItem.Animal.Lion != null)   // Lions
            {
                if (target.Item3 == AnimalTargetConstants.enemy) // Attacking
                {
                    if (subjectX != targetX)
                    {
                        if (subjectX > targetX)
                            directionXSign = DirectionConstants.negativeDirectionSign;
                        else
                            directionXSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionXSign = DirectionConstants.noDirectionSign;
                    if (subjectY != targetY)
                    {
                        if (subjectY > targetY)
                            directionYSign = DirectionConstants.negativeDirectionSign;
                        else
                            directionYSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionYSign = DirectionConstants.noDirectionSign;
                }
                else // Breeding
                {
                    if ((subjectX + gridItem.Animal.Lion.Speed) != targetX || (subjectX - gridItem.Animal.Lion.Speed) != targetX)    // x cords
                    {
                        if (subjectX - gridItem.Animal.Lion.Speed > targetX)
                            directionXSign = DirectionConstants.negativeDirectionSign;
                        else if (subjectX + gridItem.Animal.Lion.Speed < targetX)
                            directionXSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionXSign = DirectionConstants.noDirectionSign;
                    if ((subjectY + gridItem.Animal.Lion.Speed) != targetY || (subjectY - gridItem.Animal.Lion.Speed) != targetY)    // y cords
                    {
                        if (subjectY - gridItem.Animal.Lion.Speed > targetY)
                            directionYSign = DirectionConstants.negativeDirectionSign;
                        else if (subjectY + gridItem.Animal.Lion.Speed < targetY)
                            directionYSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionYSign = DirectionConstants.noDirectionSign;
                }
            }
            else // Antelopes
            {
                if (target.Item3 == AnimalTargetConstants.enemy) // Fleeing
                {
                    if (subjectX != targetX)
                    {
                        if (subjectX > targetX)
                            directionXSign = DirectionConstants.positiveDirectionSign;
                        else
                            directionXSign = DirectionConstants.negativeDirectionSign;
                    }
                    else
                        directionXSign = DirectionConstants.noDirectionSign;
                    if (subjectY != targetY)
                    {
                        if (subjectY > targetY)
                            directionYSign = DirectionConstants.positiveDirectionSign;
                        else
                            directionYSign = DirectionConstants.negativeDirectionSign;
                    }
                    else
                        directionYSign = DirectionConstants.noDirectionSign;
                }
                else // Breeding
                {
                    if ((subjectX + gridItem.Animal.Antelope?.Speed) != targetX || (subjectX - gridItem.Animal.Antelope?.Speed) != targetX)    // x cords
                    {
                        if (subjectX - gridItem.Animal.Antelope?.Speed > targetX)
                            directionXSign = DirectionConstants.negativeDirectionSign;
                        else if (subjectX + gridItem.Animal.Antelope?.Speed < targetX)
                            directionXSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionXSign = DirectionConstants.noDirectionSign;
                    if ((subjectY + gridItem.Animal.Antelope?.Speed) != targetY || (subjectY - gridItem.Animal.Antelope?.Speed) != targetY)    // y cords
                    {
                        if (subjectY - gridItem.Animal.Antelope?.Speed > targetY)
                            directionYSign = DirectionConstants.negativeDirectionSign;
                        else if (subjectY + gridItem.Animal.Antelope?.Speed < targetY)
                            directionYSign = DirectionConstants.positiveDirectionSign;
                    }
                    else
                        directionYSign = DirectionConstants.noDirectionSign;
                }
            }
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign, gridItem, target);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                if (updates.ContainsValue(coordinatesTmp))
                {
                    coordinatesTmp = updates.First(x => x.Value == coordinatesTmp).Key;
                    if ((gridTmp[coordinatesTmp].Animal.Antelope != null && gridTmp[coordinates].Animal.Antelope != null) || (gridTmp[coordinatesTmp].Animal.Lion != null && gridTmp[coordinates].Animal.Lion != null))
                    {
                        directionXSign = DirectionConstants.noDirectionSign;
                        directionYSign = DirectionConstants.noDirectionSign;
                    }
                }
                else
                {
                    if ((gridTmp[coordinatesTmp].Animal.Antelope != null && gridTmp[coordinates].Animal.Antelope != null) || (gridTmp[coordinatesTmp].Animal.Lion != null && gridTmp[coordinates].Animal.Lion != null))
                    {
                        directionXSign = DirectionConstants.noDirectionSign;
                        directionYSign = DirectionConstants.noDirectionSign;
                    }
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        // Gets the current animal count
        private int GetAnimalCount(List<GridCellModel> grid, string? type = "All")
        {
            int count = 0;
            foreach (var cell in grid)
            {
                if (type == "All")
                {
                    if (cell.Animal != null && (cell.Animal.Antelope != null || cell.Animal.Lion != null))
                        count++;
                }
                else if (type == "Antelope")
                {
                    if (cell.Animal != null && cell.Animal.Antelope != null)
                        count++;
                }
                else if (type == "Lion")
                {
                    if (cell.Animal != null && cell.Animal.Lion != null)
                        count++;
                }
            }
            return count;
        }
        // Sets empty animals to null
        private void ClearGridAnimals(List<GridCellModel> grid)
        {
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    if (cell.Animal.Antelope == null && cell.Animal.Lion == null)
                    {
                        cell.Animal = null;
                    }
                }
            }
        }
    }
}
