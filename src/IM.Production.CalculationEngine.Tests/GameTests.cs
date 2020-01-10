using CalculationEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.CalculationEngine.Tests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void Constructor_Default_InititializesCustomers()
        {
            var game = new Game();

            Assert.IsNotNull(game.Customers);
        }

        [TestMethod]
        public void Constructor_Default_InitializesTime()
        {
            var game = new Game();

            Assert.IsNotNull(game.Time);
        }

        [TestMethod]
        public void Constructor_Default_InitializesActivity()
        {
            var game = new Game();

            Assert.IsNotNull(game.Activity);
        }
    }
}
