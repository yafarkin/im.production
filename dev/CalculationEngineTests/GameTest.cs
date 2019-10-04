using CalculationEngine;
using Epam.ImitationGames.Production.Common.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dev.CalculationEngineTests
{
    [TestClass]
    public class GameTest
    {
        /// <summary>
        /// Класс должен нормальн осоздаваться и инициализироваться базовыми значениями.
        /// </summary>
        [TestMethod]
        public void Should_Create_And_Initialaze_Game_Correctly()
        {
            var game = new Game();

            Assert.IsNotNull(game.Customers);
            Assert.IsNotNull(game.Time);
            Assert.IsTrue(game.Time.Day == 0);
            Assert.IsTrue(game.TotalGameDays == 0);
            Assert.IsNotNull(game.Activity);
            Assert.IsNotNull(ReferenceData.Supply);
            Assert.IsNotNull(ReferenceData.Demand);
            Assert.IsNotNull(ReferenceData.MaterialTaxes);
            Assert.IsNotNull(ReferenceData.FactoryTaxes);
        }
    }
}
