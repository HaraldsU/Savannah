using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class InitializeGridTests
    {
        public InitializeGrid _grid;
        [TestInitialize()]
        public void Initialize()
        {
            _grid = new InitializeGrid();
        }
        [TestMethod()]
        public void InitializeTest()
        {
            // Arrange
            int dimension = 8;

            // Act
            var grid = _grid.Initialize(dimension);

            // Assert
            Assert.IsNotNull(grid);
        }
    }
}