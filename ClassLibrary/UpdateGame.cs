using AnimalLibrary.Models;
using AnimalLibrary.Models.Animals;

namespace ClassLibrary
{
    public class UpdateGame
    {

        public static List<IPlugin> _plugins = new List<IPlugin>();
        public UpdateGame(List<IPlugin> plugins)
        {
            _plugins = plugins;
        }
        // Adds a new animal depending on input
        public void AddAnimal(char animalName, List<GridCellModel> grid, bool isChild, Dictionary<int, int>? updates = null)
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

                foreach (IPlugin plugin in _plugins)
                {
                    if (animalName == plugin.FirstLetter)
                    {
                        var animal = plugin;
                        var animalModel = new AnimalsModel();
                        if (isChild)
                            animal.ActiveBreedingCooldown = animal.BreedingCooldown;

                        if (plugin.Type == "Prey")
                        {
                            animalModel.Prey = plugin.CreateNewAnimal();
                        }
                        else if (plugin.Type == "Predator")
                        {
                            animalModel.Predator = plugin.CreateNewAnimal(); ;
                        }
                        grid[cellIndex].Animal = animalModel;
                    }
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
                        // Predator moving
                        if (turn == true)
                        {
                            if (grid[coordinates].Animal.Predator != null)
                            {
                                GetAnimalNewPosition(dimension, grid, coordinates, coordinatesOld, grid[coordinates].Animal.Predator.FirstLetter, grid[coordinates].Animal.Predator.Type, updates);
                            }
                        }
                        // Prey moving
                        else
                        {
                            if (grid[coordinates].Animal.Prey != null)
                            {
                                GetAnimalNewPosition(dimension, grid, coordinates, coordinatesOld, grid[coordinates].Animal.Prey.FirstLetter, grid[coordinates].Animal.Prey.Type, updates);
                            }
                        }
                    }
                }
            }
        }
        // Gets one animal new position
        private void GetAnimalNewPosition(int dimension, List<GridCellModel> grid, int coordinates, int coordinatesOld, char animal, string animalType, Dictionary<int, int> updates)
        {
            var animalCount = GetAnimalCount(grid);
            var target = GetTarget(dimension, grid[coordinates], grid);
            var currentAnimal = animalType == "Prey" ? grid[coordinates].Animal.Prey : grid[coordinates].Animal.Predator;

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
                else if (target.Item3 == "Breed") // Other animal of same type in range
                {
                    var targetIndex = ((target.Item2 + 1) * dimension) - (dimension - target.Item1);
                    var targetAnimal = animalType == "Prey" ? grid[targetIndex].Animal.Prey : grid[targetIndex].Animal.Predator;
                    var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                    var directionXSign = directionSigns.Item1;
                    var directionYSign = directionSigns.Item2;
                    var x = grid[coordinates];
                    // Animal breeds animal
                    if (directionXSign == 'n' && directionYSign == 'n'
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
                RemoveAnimalHealth(grid[coordinatesOld].Animal, animalType);
            }
        }
        // Remove animal health
        private void RemoveAnimalHealth(AnimalsModel currentAnimal, string animalType)
        {
            if (animalType == "Predator")
                currentAnimal.Predator.Health -= .5f;
            else
                currentAnimal.Prey.Health -= .5f;
        }
        private void DeleteAnimalNoHealth(AnimalsModel keyAnimal)
        {
            if (keyAnimal.Predator != null)
            {
                if (keyAnimal.Predator.Health <= 0)
                {
                    keyAnimal.Predator = null;
                }
            }
            else if (keyAnimal.Prey != null)
            {
                if (keyAnimal.Prey.Health <= 0)
                {
                    keyAnimal.Prey = null;
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
                        if (gridItem.Animal.Predator != null && grid[coordinates].Animal.Prey != null) // Predator catching prey
                        {
                            gridItem.Animal.Predator.Health += 2;
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, "Enemy");
                        }
                        else if (gridItem.Animal.Prey != null && grid[coordinates].Animal.Predator != null) // Prey fleeing predator
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, "Enemy");
                        }
                        else if ((gridItem.Animal.Predator != null) // Predator breeding predator
                                 && (grid[coordinates].Animal.Predator != null)
                                 && (gridItem.Animal.Predator.ActiveBreedingCooldown == 0 || gridItem.Animal.Predator.ActiveBreedingCooldown > gridItem.Animal.Predator.BreedingCooldown)
                                 && (grid[coordinates].Animal.Predator.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Predator.ActiveBreedingCooldown > gridItem.Animal.Predator.BreedingCooldown)
                                 && (gridItem.Animal.Predator.FirstLetter == grid[coordinates].Animal.Predator.FirstLetter)
                                )
                        {

                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, "Breed");
                        }
                        else if ((gridItem.Animal.Prey != null)  // Prey breeding prey
                                 && (grid[coordinates].Animal.Prey != null)
                                 && (gridItem.Animal.Prey.ActiveBreedingCooldown == 0 || gridItem.Animal.Prey.ActiveBreedingCooldown > gridItem.Animal.Prey.BreedingCooldown)
                                 && (grid[coordinates].Animal.Prey.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.Prey.ActiveBreedingCooldown > gridItem.Animal.Prey.BreedingCooldown)
                                 && (gridItem.Animal.Prey.FirstLetter == grid[coordinates].Animal.Prey.FirstLetter)
                                )
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, "Breed");
                        }
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
            if (grid[coordinates].Animal.Predator != null)
            {
                steps = grid[coordinates].Animal.Predator.Speed;
                if (target != null)
                {
                    if (gridItem.X + steps > target.Item1 && directionXSign == '+')
                        steps--;
                    else if (gridItem.X - steps < target.Item1 && directionXSign == '-')
                        steps--;
                }
            }
            else
                steps = grid[coordinates].Animal.Prey.Speed;

            if (directionXSign == '-' && (coordinates - steps) >= 0 && grid[coordinates - steps].Y == grid[coordinates].Y) // Move left
                coordinates -= steps;
            else if (directionXSign == '+' && (coordinates + steps) <= grid.Count - 1 && grid[coordinates + steps].Y == grid[coordinates].Y)   // Move right
                coordinates += steps;

            // y abscissa 
            // Get steps
            if (grid[coordinatesOld].Animal.Predator != null)
            {
                steps = grid[coordinatesOld].Animal.Predator.Speed;
                // target is x, y, action
                if (target != null)
                {
                    if (gridItem.Y + steps > target.Item2 && directionYSign == '+')
                        steps--;
                    else if (gridItem.Y - steps < target.Item2 && directionYSign == '-')
                        steps--;
                }
            }
            else
                steps = grid[coordinatesOld].Animal.Prey.Speed;

            if (directionYSign == '-' && (coordinates - (dimension * steps)) >= 0) // Move up
                coordinates -= dimension * steps;
            else if (directionYSign == '+' && (coordinates + (dimension * steps)) <= grid.Count - 1)    // Move down
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
                    directionXSign = 'n';
                    directionYSign = 'n';
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        // Generates a random sign: "-", "+" or 'n' (no sign)
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
        // Generates a directional sign depending on the direction of the target: "-", "+" or 'n' (no sign)
        private Tuple<char, char> GetTargetDirectionSigns(int dimension, int coordinates, List<GridCellModel> grid, Tuple<int, int, string> target,
                                                          GridCellModel gridItem, Dictionary<int, int> updates)
        {
            char directionXSign = 'n';
            char directionYSign = 'n';
            int subjectX = gridItem.X;
            int subjectY = gridItem.Y;
            int targetX = target.Item1;
            int targetY = target.Item2;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;

            if (gridItem.Animal.Predator != null)   // Predators
            {
                if (target.Item3 == "Enemy") // Attacking
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
                else // Breeding
                {
                    if ((subjectX + 2) != targetX || (subjectX - 2) != targetX)    // x cords
                    {
                        if (subjectX - 2 > targetX)
                            directionXSign = '-';
                        else if (subjectX + 2 < targetX)
                            directionXSign = '+';
                    }
                    else
                        directionXSign = 'n';
                    if ((subjectY + 2) != targetY || (subjectY - 2) != targetY)    // y cords
                    {
                        if (subjectY - 2 > targetY)
                            directionYSign = '-';
                        else if (subjectY + 2 < targetY)
                            directionYSign = '+';
                    }
                    else
                        directionYSign = 'n';
                }
            }
            else // Preys
            {
                if (target.Item3 == "Enemy") // Fleeing
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
                else // Breeding
                {
                    if ((subjectX + 1) != targetX || (subjectX - 1) != targetX)    // x cords
                    {
                        if (subjectX - 1 > targetX)
                            directionXSign = '-';
                        else if (subjectX + 1 < targetX)
                            directionXSign = '+';
                    }
                    else
                        directionXSign = 'n';
                    if ((subjectY + 1) != targetY || (subjectY - 1) != targetY)    // y cords
                    {
                        if (subjectY - 1 > targetY)
                            directionYSign = '-';
                        else if (subjectY + 1 < targetY)
                            directionYSign = '+';
                    }
                    else
                        directionYSign = 'n';
                }
            }
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign, gridItem, target);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                if (updates.ContainsValue(coordinatesTmp))
                {
                    coordinatesTmp = updates.First(x => x.Value == coordinatesTmp).Key;
                    if ((gridTmp[coordinatesTmp].Animal.Prey != null && gridTmp[coordinates].Animal.Prey != null) || (gridTmp[coordinatesTmp].Animal.Predator != null && gridTmp[coordinates].Animal.Predator != null))
                    {
                        directionXSign = 'n';
                        directionYSign = 'n';
                    }
                }
                else
                {
                    if ((gridTmp[coordinatesTmp].Animal.Prey != null && gridTmp[coordinates].Animal.Prey != null) || (gridTmp[coordinatesTmp].Animal.Predator != null && gridTmp[coordinates].Animal.Predator != null))
                    {
                        directionXSign = 'n';
                        directionYSign = 'n';
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
                    if (cell.Animal != null && (cell.Animal.Prey != null || cell.Animal.Predator != null))
                        count++;
                }
                else if (type == "Antelope")
                {
                    if (cell.Animal != null && cell.Animal.Prey != null && cell.Animal.Prey.Name == "Antelope")
                        count++;
                }
                else if (type == "Lion")
                {
                    if (cell.Animal != null && cell.Animal.Predator != null && cell.Animal.Prey.Name == "Lion")
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
                    if (cell.Animal.Prey == null && cell.Animal.Predator == null)
                    {
                        cell.Animal = null;
                    }
                }
            }
        }
    }
}
