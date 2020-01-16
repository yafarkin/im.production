using Epam.ImitationGames.Production.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CalculationEngine;

namespace IM.Production.Domain.Tests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Constructor_Default_InitializedFactories()
        {
            var customer = new Customer();

            Assert.IsNotNull(customer.Factories);
            Assert.IsFalse(customer.Factories.Any());
        }

        [TestMethod]
        public void Constructor_Default_InitializedContracts()
        {
            var customer = new Customer();

            Assert.IsNotNull(customer.Contracts);
            Assert.IsFalse(customer.Contracts.Any());
        }

        [TestMethod]
        public void Constructor_Default_InitializedDefaultFactoryGenerationLevel()
        {
            var customer = new Customer();

            Assert.AreEqual(1, customer.FactoryGenerationLevel);
        }

        [DataTestMethod]
        [DataRow(1, 2, true)]
        [DataRow(2, 1, false)]
        [DataRow(1, 1, true)]
        public void ReadyForNextGenerationLevel_CorrectSums_ReadyOrNot(int sum, int spent, bool expected)
        {
            var game = new Game();
            var customer = new Customer();
            game.AddActivity(new CustomerRDSpentChange(game.Time, customer, spent, sum));

            var ready = customer.ReadyForNextGenerationLevel;

            Assert.AreEqual(expected, ready);
        }
    }
}
