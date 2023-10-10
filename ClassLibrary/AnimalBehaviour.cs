using AnimalLibrary.Models;
using ClassLibrary.Constants;
using ClassLibrary.Services;

namespace ClassLibrary
{
    public class AnimalBehaviour
    {
        private readonly GameService _gameService;
        public AnimalBehaviour(GameService gameService)
        {
            _gameService = gameService;
        }
        public void GetAnimalsNewPositions(int dimension, List<GridCellModel> grid, bool turn, Dictionary<int, int> updates)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var coordinatesOld = coordinates;

                    if (grid[coordinates].Animal != null)
                    {
                        // Predator moving
                        if (turn == true)
                        {
                            if (grid[coordinates].Animal.Predator != null)
                            {
                                GetAnimalNewPosition(dimension, grid, grid[coordinates].Animal.Predator, coordinates, coordinatesOld, updates);
                            }
                        }
                        // Prey moving
                        else
                        {
                            if (grid[coordinates].Animal.Prey != null)
                            {
                                GetAnimalNewPosition(dimension, grid, grid[coordinates].Animal.Prey, coordinates, coordinatesOld, updates);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets one animal new position
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="grid"></param>
        /// <param name="animal"></param>
        /// <param name="coordinates"></param>
        /// <param name="coordinatesOld"></param>
        /// <param name="updates"></param>
        private void GetAnimalNewPosition(int dimension, List<GridCellModel> grid, IPlugin animal, int coordinates, int coordinatesOld, Dictionary<int, int> updates)
        {
            var animalCount = _gameService.GetAnimalCount(grid);
            var target = GetTarget(dimension, grid[coordinates], grid);
            var currentAnimal = animal.IsPrey == Convert.ToBoolean(AnimalTypeEnums.Prey) ? grid[coordinates].Animal.Prey : grid[coordinates].Animal.Predator;

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
                else if (target.Item3 == (int)AnimalTargetEnums.MatingPartner) // Other animal of same type in range
                {
                    var targetIndex = ((target.Item2 + 1) * dimension) - (dimension - target.Item1);
                    var targetAnimal = animal.IsPrey == Convert.ToBoolean(AnimalTypeEnums.Prey) ? grid[targetIndex].Animal.Prey : grid[targetIndex].Animal.Predator;
                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                    var directionXSign = directionSigns.Item1;
                    var directionYSign = directionSigns.Item2;
                    var x = grid[coordinates];
                    // Animal breeds animal
                    if (directionXSign == (int)DirectionEnums.NoDirectionSign && directionYSign == (int)DirectionEnums.NoDirectionSign
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
                        ClearGridAnimals(grid);
                        _gameService.AddAnimal(animal, pressedKey: ConsoleKey.NoName, grid, isChild: true, updates);
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
                RemoveAnimalHealth(grid[coordinatesOld].Animal, animal.IsPrey);
            }
        }
        /// <summary>
        /// Calls "GetTargetForLoop" with the appropriate variables or returns Tuple(-1, -1, string.Empty)
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="gridItem"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        private Tuple<int, int, int> GetTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid)
        {
            dimension--;
            int range = 0;

            if (gridItem.Animal != null)
            {
                if (gridItem.Animal.Predator != null)
                    range = gridItem.Animal.Predator.Range;
                else if (gridItem.Animal.Prey != null)
                    range = gridItem.Animal.Prey.Range;
            }

            int widthStart = Math.Max(0, gridItem.X - range);
            int heightStart = Math.Max(0, gridItem.Y - range);
            int widthEnd = Math.Min(dimension, gridItem.X + range);
            int heightEnd = Math.Min(dimension, gridItem.Y + range);

            if (gridItem.Animal != null)
            {
                if (gridItem.Animal.Predator != null)
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            heightStart, heightEnd, widthStart, widthEnd
                                           );
                else if (gridItem.Animal.Prey != null)
                    return GetTargetForLoop(dimension, gridItem, grid,
                                            heightStart, heightEnd, widthStart, widthEnd
                                           );
            }
            else
                return Tuple.Create(-1, -1, -1);

            return Tuple.Create(-1, -1, -1);
        }
        /// <summary>
        /// Finds the animal "Target"
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="gridItem"></param>
        /// <param name="grid"></param>
        /// <param name="heightStart"></param>
        /// <param name="heightEnd"></param>
        /// <param name="widthStart"></param>
        /// <param name="widthEnd"></param>
        /// <returns></returns>
        private Tuple<int, int, int> GetTargetForLoop(
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
                        if (gridItem.Animal.Predator != null && grid[coordinates].Animal.Prey != null) // Predator catching prey
                        {
                            gridItem.Animal.Predator.Health += 2;
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, (int)AnimalTargetEnums.Enemy);
                        }
                        else if (gridItem.Animal.Prey != null && grid[coordinates].Animal.Predator != null) // Prey fleeing predator
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, (int)AnimalTargetEnums.Enemy);
                        else if (gridItem.Animal.Predator != null // Predator breeding predator
                                 && grid[coordinates].Animal.Predator != null
                                 && (gridItem.Animal.Predator.ActiveBreedingCooldown == 0 || gridItem.Animal.Predator.ActiveBreedingCooldown > gridItem.Animal.Predator.BreedingCooldown)
                                 && (grid[coordinates].Animal.Predator.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Predator.ActiveBreedingCooldown > gridItem.Animal.Predator.BreedingCooldown)
                                 && (gridItem.Animal.Predator.FirstLetter == grid[coordinates].Animal.Predator.FirstLetter)
                                )
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, (int)AnimalTargetEnums.MatingPartner);
                        else if (gridItem.Animal.Prey != null  // Prey breeding prey
                                 && grid[coordinates].Animal.Prey != null
                                 && (gridItem.Animal.Prey.ActiveBreedingCooldown == 0 || gridItem.Animal.Prey.ActiveBreedingCooldown > gridItem.Animal.Prey.BreedingCooldown)
                                 && (grid[coordinates].Animal.Prey.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Prey.ActiveBreedingCooldown > gridItem.Animal.Prey.BreedingCooldown)
                                 && (gridItem.Animal.Prey.FirstLetter == grid[coordinates].Animal.Prey.FirstLetter)
                                )
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, (int)AnimalTargetEnums.MatingPartner);
                    }
                }
            }
            return Tuple.Create(-1, -1, -1);
        }
        /// <summary>
        /// Gets the new animal coordinates
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="grid"></param>
        /// <param name="coordinates"></param>
        /// <param name="directionXSign"></param>
        /// <param name="directionYSign"></param>
        /// <param name="gridItem"></param>
        /// <param name="target"></param>
        private void MoveAnimalPosition(int dimension, List<GridCellModel> grid, ref int coordinates, int directionXSign, int directionYSign,
                                        GridCellModel? gridItem = null, Tuple<int, int, int>? target = null)
        {
            int steps;
            int coordinatesOld = coordinates;
            var animalTmp = grid[coordinatesOld].Animal;
            IPlugin animal = new AnimalsModel().Predator;
            if (animalTmp.Predator != null)
                animal = animalTmp.Predator;
            else if (animalTmp.Prey != null)
                animal = animalTmp.Prey;

            // x abscissa 
            // Get steps
            steps = animal.Speed;
            if (target != null)
            {
                if (animalTmp.Predator != null)
                {
                    if (target.Item3 == (int)AnimalTargetEnums.MatingPartner)
                    {
                        if (gridItem.X + steps >= target.Item1 && directionXSign == (int)DirectionEnums.PositiveDirectionSign)
                            steps--;
                        else if (gridItem.X - steps <= target.Item1 && directionXSign == (int)DirectionEnums.NegativeDirectionSign)
                            steps--;
                    }
                    else
                    {
                        if (gridItem.X + steps > target.Item1 && directionXSign == (int)DirectionEnums.PositiveDirectionSign)
                            steps--;
                        else if (gridItem.X - steps < target.Item1 && directionXSign == (int)DirectionEnums.NegativeDirectionSign)
                            steps--;
                    }

                }
            }

            if (directionXSign == (int)DirectionEnums.NegativeDirectionSign && (coordinates - steps) >= 0 && grid[coordinates - steps].Y == grid[coordinates].Y) // Move left
                coordinates -= steps;
            else if (directionXSign == (int)DirectionEnums.PositiveDirectionSign && (coordinates + steps) <= grid.Count - 1 && grid[coordinates + steps].Y == grid[coordinates].Y)   // Move right
                coordinates += steps;

            // y abscissa 
            // Get steps
            steps = animal.Speed;
            // target is Tuple(x, y, action)
            if (target != null)
            {
                if (animalTmp.Predator != null)
                {
                    if (target.Item3 == (int)AnimalTargetEnums.MatingPartner)
                    {
                        if (gridItem.Y + steps >= target.Item2 && directionYSign == (int)DirectionEnums.PositiveDirectionSign)
                            steps--;
                        else if (gridItem.Y - steps <= target.Item2 && directionYSign == (int)DirectionEnums.NegativeDirectionSign)
                            steps--;
                    }
                    else
                    {
                        if (gridItem.Y + steps > target.Item2 && directionYSign == (int)DirectionEnums.PositiveDirectionSign)
                            steps--;
                        else if (gridItem.Y - steps < target.Item2 && directionYSign == (int)DirectionEnums.NegativeDirectionSign)
                            steps--;
                    }
                }
            }

            if (directionYSign == (int)DirectionEnums.NegativeDirectionSign && (coordinates - (dimension * steps)) >= 0) // Move up
                coordinates -= dimension * steps;
            else if (directionYSign == (int)DirectionEnums.PositiveDirectionSign && (coordinates + (dimension * steps)) <= grid.Count - 1)    // Move down
                coordinates += dimension * steps;
        }
        /// <summary>
        /// Generates a random directional sign: "-", "+" or 'n' (no sign). Defaults as 'n' if fails after 8 tries
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="grid"></param>
        /// <param name="coordinates"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        private Tuple<int, int> GetRandomDirectionSigns(int dimension, List<GridCellModel> grid, int coordinates, Dictionary<int, int> updates)
        {
            int count = 0;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;
            int directionXSign = 0;
            int directionYSign = 0;

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
                    directionXSign = (int)DirectionEnums.NoDirectionSign;
                    directionYSign = (int)DirectionEnums.NoDirectionSign;
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        /// <summary>
        /// Generates a random sign: "-", "+" or 'n' (no sign)
        /// </summary>
        /// <param name="directionXSign"></param>
        /// <param name="directionYSign"></param>
        private void GenerateRandomSign(ref int directionXSign, ref int directionYSign)
        {
            int directionX = RandomGenerator.Next(3);
            if (directionX == 0) directionXSign = (int)DirectionEnums.NegativeDirectionSign;
            else if (directionX == 1) directionXSign = (int)DirectionEnums.PositiveDirectionSign;
            else directionXSign = (int)DirectionEnums.NoDirectionSign;

            if (directionXSign == (int)DirectionEnums.NoDirectionSign)
            {
                int directionY = RandomGenerator.Next(2);
                if (directionY == 0) directionYSign = (int)DirectionEnums.NegativeDirectionSign;
                else directionYSign = (int)DirectionEnums.PositiveDirectionSign;
            }
            else
            {
                int directionY = RandomGenerator.Next(3);
                if (directionY == 0) directionYSign = (int)DirectionEnums.NegativeDirectionSign;
                else if (directionY == 1) directionYSign = (int)DirectionEnums.PositiveDirectionSign;
                else directionYSign = (int)DirectionEnums.NoDirectionSign;

                if (directionYSign == (int)DirectionEnums.NoDirectionSign)
                {
                    directionX = RandomGenerator.Next(2);
                    if (directionX == 0) directionXSign = (int)DirectionEnums.NegativeDirectionSign;
                    else if (directionX == 1) directionXSign = (int)DirectionEnums.PositiveDirectionSign;
                }
            }
        }
        /// <summary>
        /// Generates a directional sign depending on the direction of the target: "-", "+" or DirectionEnums.noDirectionSign (no sign)
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="coordinates"></param>
        /// <param name="grid"></param>
        /// <param name="target"></param>
        /// <param name="gridItem"></param>
        /// <param name="updates"></param>
        /// <returns></returns>
        private Tuple<int, int> GetTargetDirectionSigns(int dimension, int coordinates, List<GridCellModel> grid, Tuple<int, int, int> target,
                                                          GridCellModel gridItem, Dictionary<int, int> updates)
        {
            int directionXSign = (int)DirectionEnums.NoDirectionSign;
            int directionYSign = (int)DirectionEnums.NoDirectionSign;
            // Subject animal coordinates
            int subjectX = gridItem.X;
            int subjectY = gridItem.Y;
            // Target animal coordinates
            int targetX = target.Item1;
            int targetY = target.Item2;

            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;

            if (gridItem.Animal.Predator != null)   // Predators
            {
                if (target.Item3 == (int)AnimalTargetEnums.Enemy) // Attacking
                {
                    if (subjectX != targetX)
                    {
                        if (subjectX > targetX)
                            directionXSign = (int)DirectionEnums.NegativeDirectionSign;
                        else
                            directionXSign = (int)DirectionEnums.PositiveDirectionSign;
                    }
                    else
                        directionXSign = (int)DirectionEnums.NoDirectionSign;
                    if (subjectY != targetY)
                    {
                        if (subjectY > targetY)
                            directionYSign = (int)DirectionEnums.NegativeDirectionSign;
                        else
                            directionYSign = (int)DirectionEnums.PositiveDirectionSign;
                    }
                    else
                        directionYSign = (int)DirectionEnums.NoDirectionSign;
                }
                else // Breeding
                {
                    SetTargetBreedingDirectionSigns(subjectX, targetX, subjectY, targetY, ref directionXSign, ref directionYSign);
                }
            }
            else // Preys
            {
                if (target.Item3 == (int)AnimalTargetEnums.Enemy) // Fleeing
                {
                    if (subjectX != targetX)
                    {
                        if (subjectX > targetX)
                            directionXSign = (int)DirectionEnums.PositiveDirectionSign;
                        else
                            directionXSign = (int)DirectionEnums.NegativeDirectionSign;
                    }
                    else
                        directionXSign = (int)DirectionEnums.NoDirectionSign;
                    if (subjectY != targetY)
                    {
                        if (subjectY > targetY)
                            directionYSign = (int)DirectionEnums.PositiveDirectionSign;
                        else
                            directionYSign = (int)DirectionEnums.NegativeDirectionSign;
                    }
                    else
                        directionYSign = (int)DirectionEnums.NoDirectionSign;
                }
                else // Breeding
                {
                    SetTargetBreedingDirectionSigns(subjectX, targetX, subjectY, targetY, ref directionXSign, ref directionYSign);
                }
            }
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign, gridItem, target);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                if (updates.ContainsValue(coordinatesTmp))
                {
                    coordinatesTmp = updates.First(x => x.Value == coordinatesTmp).Key;
                }
                if ((gridTmp[coordinatesTmp].Animal.Prey != null && gridTmp[coordinates].Animal.Prey != null) || (gridTmp[coordinatesTmp].Animal.Predator != null && gridTmp[coordinates].Animal.Predator != null))
                {
                    directionXSign = (int)DirectionEnums.NoDirectionSign;
                    directionYSign = (int)DirectionEnums.NoDirectionSign;
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        private void SetTargetBreedingDirectionSigns(int subjectX, int targetX, int subjectY, int targetY, ref int directionXSign, ref int directionYSign)
        {
            if (Math.Abs(subjectX - targetX) != 1 && Math.Abs(subjectX + targetX) != 1)
            {
                if (subjectX != targetX)    // x cords
                {
                    if (subjectX - 1 >= targetX)
                        directionXSign = (int)DirectionEnums.NegativeDirectionSign;
                    else if (subjectX + 1 <= targetX)
                        directionXSign = (int)DirectionEnums.PositiveDirectionSign;
                }
                else
                    directionXSign = (int)DirectionEnums.NoDirectionSign;
            }
            if (Math.Abs(subjectY - targetY) != 1 && Math.Abs(subjectY + targetY) != 1)
            {
                if (subjectY != targetY)    // y cords
                {
                    if (subjectY - 1 >= targetY)
                        directionYSign = (int)DirectionEnums.NegativeDirectionSign;
                    else if (subjectY + 1 <= targetY)
                        directionYSign = (int)DirectionEnums.PositiveDirectionSign;
                }
                else
                    directionYSign = (int)DirectionEnums.NoDirectionSign;
            }
        }
        /// <summary>
        /// Sets empty animals to null
        /// </summary>
        /// <param name="grid"></param>
        private void ClearGridAnimals(List<GridCellModel> grid)
        {
            foreach (var cell in grid)
            {
                if (cell.Animal != null)
                {
                    if (cell.Animal.Prey == null && cell.Animal.Predator == null)
                    {
                        cell.Animal = null;
                    }
                }
            }
        }
        private void RemoveAnimalHealth(AnimalsModel currentAnimal, bool animalType)
        {
            if (animalType == Convert.ToBoolean(AnimalTypeEnums.Predator))
                currentAnimal.Predator.Health -= 1;
            else
                currentAnimal.Prey.Health -= 1;
        }
    }
}
