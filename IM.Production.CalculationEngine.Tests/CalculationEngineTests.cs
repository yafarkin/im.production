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
        public void Calculate_NoContractsWithGame_SumNotChanged()
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
        public void Calculate_ContractWithGameAndNoMaterialOnStock_NewMaterialAdded()
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
        public void Calculate_ContractWithGameAndMaterialOnStock_MaterialAmountIncreased()
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

        [TestMethod]
        public void Calculate_SumForNextGenerationLevelIsZero_SetCorrectSum()
        {
            var customer = new Customer { SumToNextGenerationLevel = 0, FactoryGenerationLevel = 1 };
            var game = new Game();
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(5000, customer.SumToNextGenerationLevel);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Calculate_SumOnRDIsLowerThanZero_SumsNotChanged(int sumOnRD)
        {
            var sum = 1;
            var spent = 1;
            var customer = new Customer { SumOnRD = sumOnRD, Sum = sum, SpentSumToNextGenerationLevel = spent };
            var game = new Game();
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(sum, customer.Sum);
            Assert.AreEqual(spent, customer.SpentSumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_ReadyForNextGenerationLevel_GenerationLevelIncreasedAndSumsChanged()
        {
            var customer = new Customer { Sum = 10000, SumOnRD = 6000, FactoryGenerationLevel = 1 };
            var game = new Game();
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(2, customer.FactoryGenerationLevel);
            Assert.AreEqual(4000, customer.Sum);
            Assert.AreEqual(1000, customer.SpentSumToNextGenerationLevel);
            Assert.AreEqual(15000, customer.SumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_PositiveSumOnRDAndNotReady_SumsChangedAndGenerationLevelAndSumToNextNotChanged()
        {
            const int generationLevel = 1;
            const int sumToNext = 5000;
            var customer = new Customer { Sum = 10, SumOnRD = 1, SpentSumToNextGenerationLevel = 2, FactoryGenerationLevel = generationLevel, SumToNextGenerationLevel = sumToNext };
            var game = new Game();
            game.Customers.Add(customer);

            _calculationEgnine.Calculate(game);

            Assert.AreEqual(9, customer.Sum);
            Assert.AreEqual(3, customer.SpentSumToNextGenerationLevel);
            Assert.AreEqual(generationLevel, customer.FactoryGenerationLevel);
            Assert.AreEqual(sumToNext, customer.SumToNextGenerationLevel);
        }
    }
}