using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Services;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class GridServiceTests
    {
        private GridService _grid;
        [TestInitialize()]
        public void Initialize()
        {
            _grid = new GridService();
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