using Savanna.Commons.Enums;
using Savanna.Data;
using Savanna.Data.Base;
using Savanna.Data.Interfaces;
using Savanna.Data.Models;
using Savanna.Services.Helper;

namespace Savanna.Services
{
    public class AnimalBehaviour
    {
        private readonly GameService _gameService;
        private int gameId;
        private int sessionId;

        public AnimalBehaviour(GameService gameService)
        {
            _gameService = gameService;
        }

        public void GetAnimalsNewPositions(int dimension, List<GridCellModel> grid, AnimalTypeEnums turn, Dictionary<int, int> updates, int gameId, int sessionId)
        {
            this.gameId = gameId;
            this.sessionId = sessionId;

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var coordinatesOld = coordinates;

                    if (grid[coordinates].Animal != null)
                    {
                        if (grid[coordinates].Animal.AnimalType == turn)
                        {
                            GetAnimalNewPosition(dimension, grid, grid[coordinates].Animal, coordinates, coordinatesOld, updates);
                        }
                    }
                }
            }
        }
        private void GetAnimalNewPosition(int dimension, List<GridCellModel> grid, IAnimalProperties animal, int coordinates, int coordinatesOld, Dictionary<int, int> updates)
        {
            var animalCount = _gameService.GetAnimalCount(grid);
            var target = GetTarget(dimension, grid[coordinates], grid); // X, Y, target: (Enemy, mating partner)

            ResetBreedingCooldown(animal);
            // No target animals in range
            if (target.Item3 == AnimalTargetEnums.NoTarget)
            {
                GetAnimalNewPositionNoTarget(dimension, grid, ref coordinates, updates);
            }
            // Animal of same type in range
            else if (target.Item3 == AnimalTargetEnums.MatingPartner)
            {
                GetAnimalNewPositionMatingPartner(dimension, grid, ref coordinates, updates, target, animal, animalCount);
            }
            // Target animal in range
            else
            {
                GetAnimalNewPositionEnemy(dimension, grid, ref coordinates, updates, target);
            }

            if (!updates.ContainsKey(coordinatesOld))
            {
                updates.Add(coordinatesOld, coordinates);
            }
            RemoveAnimalHealth(grid[coordinatesOld].Animal);
        }
        private Tuple<int, int, AnimalTargetEnums> GetTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid)
        {
            dimension--;
            if (gridItem.Animal != null)
            {
                var range = gridItem.Animal.Range;
                int widthStart = Math.Max(0, gridItem.X - range);
                int heightStart = Math.Max(0, gridItem.Y - range);
                int widthEnd = Math.Min(dimension, gridItem.X + range);
                int heightEnd = Math.Min(dimension, gridItem.Y + range);

                return FindTarget(dimension, gridItem, grid, heightStart, heightEnd, widthStart, widthEnd);
            }
            else
            {
                return Tuple.Create(-1, -1, AnimalTargetEnums.NoTarget);
            }
        }
        private Tuple<int, int, AnimalTargetEnums> FindTarget(int dimension, GridCellModel gridItem, List<GridCellModel> grid, int heightStart,
                                            int heightEnd, int widthStart, int widthEnd)
        {
            dimension++;
            // Height (Y)
            for (int i = heightStart; i < heightEnd + 1; i++)
            {
                // Width (X)
                for (int j = widthStart; j < widthEnd + 1; j++)
                {
                    var coordinates = ((i + 1) * dimension) - (dimension - j);
                    var gridItemCoordinates = ((gridItem.Y + 1) * dimension) - (dimension - gridItem.X);
                    if (grid[coordinates].Animal != null && coordinates != gridItemCoordinates)
                    {
                        // Animal finding animal of different type
                        if (gridItem.Animal.AnimalType != grid[coordinates].Animal.AnimalType)
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetEnums.Enemy);
                        }
                        // Animal breeding animal
                        else if (gridItem.Animal.AnimalType == grid[coordinates].Animal.AnimalType &&
                                 (gridItem.Animal.ActiveBreedingCooldown == 0 || gridItem.Animal.ActiveBreedingCooldown > gridItem.Animal.BreedingCooldown) &&
                                 (grid[coordinates].Animal.ActiveBreedingCooldown == 0 || grid[coordinates].Animal.ActiveBreedingCooldown > gridItem.Animal.BreedingCooldown) &&
                                 (gridItem.Animal.Name == grid[coordinates].Animal.Name)
                                )
                        {
                            return Tuple.Create(grid[coordinates].X, grid[coordinates].Y, AnimalTargetEnums.MatingPartner);
                        }
                    }
                }
            }
            return Tuple.Create(-1, -1, AnimalTargetEnums.NoTarget);
        }
        private void ResetBreedingCooldown(IAnimalProperties animal)
        {
            if (animal.ActiveBreedingCooldown != 0)
            {
                animal.ActiveBreedingCooldown--;
                if (animal.ActiveBreedingCooldown == animal.BreedingCooldown)
                {
                    animal.IsBirthing = false;
                }
            }
        }
        private void GetAnimalNewPositionNoTarget(int dimension, List<GridCellModel> grid, ref int coordinates, Dictionary<int, int> updates)
        {
            var directionSigns = GetRandomDirectionSigns(dimension, grid, coordinates, updates);
            var directionXSign = directionSigns.Item1;
            var directionYSign = directionSigns.Item2;
            MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign);
        }
        private void GetAnimalNewPositionMatingPartner(int dimension, List<GridCellModel> grid, ref int coordinates, Dictionary<int, int> updates,
                                                       Tuple<int, int, AnimalTargetEnums> target, IAnimalProperties animal, int animalCount)
        {
            var targetIndex = ((target.Item2 + 1) * dimension) - (dimension - target.Item1);
            var targetAnimal = grid[targetIndex].Animal;
            var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
            var directionXSign = directionSigns.Item1;
            var directionYSign = directionSigns.Item2;
            var x = grid[coordinates];
            // Animal breeds animal
            if (directionXSign == DirectionEnums.NoDirectionSign && directionYSign == DirectionEnums.NoDirectionSign
                && (animal.ActiveBreedingCooldown == 0 && targetAnimal.ActiveBreedingCooldown == 0))
            {
                animal.ActiveBreedingCooldown = animal.BreedingTime + animal.BreedingCooldown;
                animal.IsBirthing = true;
                targetAnimal.ActiveBreedingCooldown = targetAnimal.BreedingTime + targetAnimal.BreedingCooldown;
            }
            else if (animal.ActiveBreedingCooldown == animal.BreedingCooldown + 1 && animal.IsBirthing == true)
            {
                if ((updates.Count + animalCount) >= grid.Count)
                {
                    var updatesElement = updates.Where(a => a.Key != a.Value).FirstOrDefault();
                    updates.Remove(updatesElement.Key);
                    updates.Add(updatesElement.Key, updatesElement.Key);
                }
                _gameService.AddAnimal(gameId, sessionId, animal.Name, isChild: true, updates);
                directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
                directionXSign = directionSigns.Item1;
                directionXSign = directionSigns.Item2;
            }
            MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
        }
        private void GetAnimalNewPositionEnemy(int dimension, List<GridCellModel> grid, ref int coordinates, Dictionary<int, int> updates,
                                               Tuple<int, int, AnimalTargetEnums> target)
        {
            var directionSigns = GetTargetDirectionSigns(dimension, coordinates, grid, target, grid[coordinates], updates);
            var directionXSign = directionSigns.Item1;
            var directionYSign = directionSigns.Item2;
            MoveAnimalPosition(dimension, grid, ref coordinates, directionXSign, directionYSign, grid[coordinates], target);
        }
        private Tuple<DirectionEnums, DirectionEnums> GetRandomDirectionSigns(int dimension, List<GridCellModel> grid, int coordinates, Dictionary<int, int> updates)
        {
            int count = 0;
            var gridTmp = grid;
            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            DirectionEnums directionXSign = DirectionEnums.NoDirectionSign;
            DirectionEnums directionYSign = DirectionEnums.NoDirectionSign;

            GenerateRandomSigns(ref directionXSign, ref directionYSign);
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                do
                {
                    GenerateRandomSigns(ref directionXSign, ref directionYSign);
                    coordinatesTmp = coordinates;
                    MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign); // Get new coordinates
                    count++;
                } while ((coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp)) && count <= 8);
                if (count >= 8)
                {
                    directionXSign = DirectionEnums.NoDirectionSign;
                    directionYSign = DirectionEnums.NoDirectionSign;
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        private Tuple<DirectionEnums, DirectionEnums> GetTargetDirectionSigns(int dimension, int coordinates, List<GridCellModel> grid, Tuple<int, int, AnimalTargetEnums> target,
                                                                              GridCellModel gridItem, Dictionary<int, int> updates)
        {
            DirectionEnums directionXSign = DirectionEnums.NoDirectionSign;
            DirectionEnums directionYSign = DirectionEnums.NoDirectionSign;

            // Subject animal coordinates
            int subjectX = gridItem.X;
            int subjectY = gridItem.Y;

            // Target animal coordinates
            int targetX = target.Item1;
            int targetY = target.Item2;

            int coordinatesOld = coordinates;
            int coordinatesTmp = coordinates;
            var gridTmp = grid;

            if (target.Item3 == AnimalTargetEnums.Enemy)
            {
                AnimalBase animal = AnimalFactory.CreateAnimal(gridItem.Animal.AnimalType);
                animal.SetDirectionSigns(subjectX, subjectY, targetX, targetY, ref directionXSign, ref directionYSign);
            }
            else
            {
                SetTargetBreedingDirectionSigns(subjectX, targetX, subjectY, targetY, ref directionXSign, ref directionYSign);
            }
            MoveAnimalPosition(dimension, gridTmp, ref coordinatesTmp, directionXSign, directionYSign, gridItem, target);
            if (coordinatesTmp == coordinatesOld || gridTmp[coordinatesTmp].Animal != null || updates.ContainsValue(coordinatesTmp))
            {
                if (updates.ContainsValue(coordinatesTmp))
                {
                    coordinatesTmp = updates.First(x => x.Value == coordinatesTmp).Key;
                }
                if (gridTmp[coordinatesTmp].Animal.AnimalType == gridTmp[coordinates].Animal.AnimalType)
                {
                    directionXSign = DirectionEnums.NoDirectionSign;
                    directionYSign = DirectionEnums.NoDirectionSign;
                }
            }
            var returnData = Tuple.Create(directionXSign, directionYSign);

            return returnData;
        }
        private void GenerateRandomSigns(ref DirectionEnums directionXSign, ref DirectionEnums directionYSign)
        {
            int directionX = RandomGenerator.Next(2);
            if (directionX == 0)
            {
                directionXSign = DirectionEnums.NegativeDirectionSign;
            }
            else if (directionX == 1)
            {
                directionXSign = DirectionEnums.PositiveDirectionSign;
            }

            int directionY = RandomGenerator.Next(2);
            if (directionY == 0)
            {
                directionYSign = DirectionEnums.NegativeDirectionSign;
            }
            else if (directionY == 1)
            {
                directionYSign = DirectionEnums.PositiveDirectionSign;
            }
        }
        private void SetTargetBreedingDirectionSigns(int subjectX, int targetX, int subjectY, int targetY, ref DirectionEnums directionXSign, ref DirectionEnums directionYSign)
        {
            SetTargetBreedingDirectionSign(subjectX, targetX, ref directionXSign);
            SetTargetBreedingDirectionSign(subjectY, targetY, ref directionYSign);
        }
        private void SetTargetBreedingDirectionSign(int subject, int target, ref DirectionEnums directionSign)
        {
            if (Math.Abs(subject - target) != 1 && Math.Abs(subject + target) != 1)
            {
                if (subject != target)
                {
                    if (subject - 1 >= target)
                    {
                        directionSign = DirectionEnums.NegativeDirectionSign;
                    }
                    else if (subject + 1 <= target)
                    {
                        directionSign = DirectionEnums.PositiveDirectionSign;
                    }
                }
                else
                {
                    directionSign = DirectionEnums.NoDirectionSign;
                }
            }
        }
        private void MoveAnimalPosition(int dimension, List<GridCellModel> grid, ref int coordinates, DirectionEnums directionXSign, DirectionEnums directionYSign,
                                         GridCellModel? gridItem = null, Tuple<int, int, AnimalTargetEnums>? target = null)
        {
            int coordinatesOld = coordinates;
            var animal = grid[coordinatesOld].Animal;
            int steps = animal.Speed;

            // x abscissa 
            if (target != null)
            {
                if (steps > 1)
                {
                    CalculateSteps(ref steps, directionXSign, gridItem, target, true);
                }
            }

            // Move left
            if (directionXSign == DirectionEnums.NegativeDirectionSign && (coordinates - steps) >= 0 && grid[coordinates - steps].Y == grid[coordinates].Y)
            {
                coordinates -= steps;
            }
            // Move right
            else if (directionXSign == DirectionEnums.PositiveDirectionSign && (coordinates + steps) <= grid.Count - 1 && grid[coordinates + steps].Y == grid[coordinates].Y)
            {
                coordinates += steps;
            }

            steps = animal.Speed;
            // y abscissa 
            // target is Tuple(x, y, action)
            if (target != null)
            {
                if (steps > 1)
                {
                    CalculateSteps(ref steps, directionYSign, gridItem, target, false);
                }
            }

            // Move up
            if (directionYSign == DirectionEnums.NegativeDirectionSign && (coordinates - (dimension * steps)) >= 0)
            {
                coordinates -= dimension * steps;
            }
            // Move down
            else if (directionYSign == DirectionEnums.PositiveDirectionSign && (coordinates + (dimension * steps)) <= grid.Count - 1)
            {
                coordinates += dimension * steps;
            }
        }
        private void CalculateSteps(ref int steps, DirectionEnums directionSign, GridCellModel gridItem, Tuple<int, int, AnimalTargetEnums> target, bool isXCoordinate)
        {
            var subjectCoordinate = isXCoordinate == true ? gridItem.X : gridItem.Y;
            var targetCoordinate = isXCoordinate == true ? target.Item1 : target.Item2;
            if (target.Item3 == AnimalTargetEnums.MatingPartner)
            {
                if (subjectCoordinate + steps >= targetCoordinate && directionSign == DirectionEnums.PositiveDirectionSign)
                {
                    steps--;
                }
                else if (subjectCoordinate - steps <= targetCoordinate && directionSign == DirectionEnums.NegativeDirectionSign)
                {
                    steps--;
                }
            }
            else
            {
                if (subjectCoordinate + steps > targetCoordinate && directionSign == DirectionEnums.PositiveDirectionSign)
                {
                    steps--;
                }
                else if (subjectCoordinate - steps < targetCoordinate && directionSign == DirectionEnums.NegativeDirectionSign)
                {
                    steps--;
                }
            }
        }
        private void RemoveAnimalHealth(IAnimalProperties currentAnimal)
        {
            currentAnimal.Health -= 1;
        }
    }
}
