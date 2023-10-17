using Microsoft.VisualStudio.TestTools.UnitTesting;
using Savanna.Services;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class RandomGeneratorTests
    {
        [TestMethod()]
        public void NextTest()
        {
            var random = RandomGenerator.Next(10);
            Assert.IsNotNull(random);
        }
    }
}