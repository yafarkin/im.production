using CalculationEngine;
using Epam.ImitationGames.Production.Common;
using Epam.ImitationGames.Production.Common.Production;
using Epam.ImitationGames.Production.Common.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IM.Production.CalculationEngine.Tests
{
    [TestClass]
    public class CalculationEngineTests
    {
        private CalculationEgnine _calculationEgnine;

        [TestInitialize]
        public void TestInitialize()
        {
            _calculationEgnine = new CalculationEgnine();
            _calculationEgnine.Game = new Game();
        }

        [TestMethod]
        public void Calculate_NoContractsWithGame_KeepsSum()
        {
            var sum = 0;
            var customer = new Customer { Sum = sum };
            var contract = new Contract(null, null) { SourceFactory = new Factory() };
            var game = new Game();
            customer.Contracts.Add(contract);
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(sum, customer.Sum);
        }

        [TestMethod]
        public void Calculate_ContractWithGameAndNoMaterialOnStock_DeliversNewMaterial()
        {
            var material = ReferenceData.GetMaterialByKey(MaterialKeys.MetalRuda);
            var amount = 100;
            var customer = new Customer { Sum = 10 };
            var factory = new Factory { Customer = customer };
            var contract = new Contract(null, new MaterialWithPrice { Material = material, Amount = amount }) { DestinationFactory = factory };
            var game = new Game();
            customer.Contracts.Add(contract);
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            var materialOnStock = factory.Stock.First();
            Assert.AreSame(material, materialOnStock.Material);
            Assert.AreEqual(amount, materialOnStock.Amount);
            Assert.AreEqual(9, customer.Sum);
        }

        [TestMethod]
        public void Calculate_ContractWithGameAndMaterialOnStock_IncreasesMaterialAmount()
        {
            var material = ReferenceData.GetMaterialByKey(MaterialKeys.MetalRuda);
            var customer = new Customer { Sum = 10 };
            var factory = new Factory() { Customer = customer };
            var contract = new Contract(null, new MaterialWithPrice { Amount = 100, Material = material }) { DestinationFactory = factory };
            var game = new Game();
            var materialOnStock = new MaterialOnStock { Amount = 1, Material = material };
            factory.Stock.Add(materialOnStock);
            customer.Contracts.Add(contract);
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(101, materialOnStock.Amount);
            Assert.AreEqual(9, customer.Sum);
        }
    }
}