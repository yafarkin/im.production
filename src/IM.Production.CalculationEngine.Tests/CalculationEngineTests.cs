using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Production.CalculationEngine.Tests
{
    [TestClass]
    public class CalculationEngineTests
    {
        private CalculationEngine _calculationEngine;
        private Game _game;

        [TestInitialize]
        public void TestInitialize()
        {
            _game = new Game();
            _calculationEngine = new CalculationEngine(_game);
        }

        [TestMethod]
        public void Calculate_NoContractsWithGame_SumNotChanged()
        {
            var sum = 0;
            var customer = new Customer {Sum = sum};
            var contract = new Contract(null, null) {SourceFactory = new Factory()};
            customer.Contracts.Add(contract);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

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
            customer.Contracts.Add(contract);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            var materialOnStock = factory.Stock.First();
            Assert.AreSame(material, materialOnStock.Material);
            Assert.AreEqual(amount, materialOnStock.Amount);
            Assert.AreEqual(9, customer.Sum);
        }

        [TestMethod]
        public void Calculate_ContractWithGameAndMaterialOnStock_MaterialAmountIncreased()
        {
            var material = ReferenceData.GetMaterialByKey(MaterialKeys.MetalRuda);
            var customer = new Customer {Sum = 10};
            var factory = new Factory {Customer = customer};
            var contract =
                new Contract(null, new MaterialWithPrice {Amount = 100, Material = material})
                {
                    DestinationFactory = factory
                };
            var materialOnStock = new MaterialOnStock {Amount = 1, Material = material};
            factory.Stock.Add(materialOnStock);
            customer.Contracts.Add(contract);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(101, materialOnStock.Amount);
            Assert.AreEqual(9, customer.Sum);
        }

        [TestMethod]
        public void Calculate_SumForNextGenerationLevelIsZero_SetCorrectSum()
        {
            var customer = new Customer { SumToNextGenerationLevel = 0, FactoryGenerationLevel = 1 };
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

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
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(sum, customer.Sum);
            Assert.AreEqual(spent, customer.SpentSumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_ReadyForNextGenerationLevel_GenerationLevelIncreasedAndSumsChanged()
        {
            var customer = new Customer { Sum = 10000, SumOnRD = 6000, FactoryGenerationLevel = 1 };
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

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
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(9, customer.Sum);
            Assert.AreEqual(3, customer.SpentSumToNextGenerationLevel);
            Assert.AreEqual(generationLevel, customer.FactoryGenerationLevel);
            Assert.AreEqual(sumToNext, customer.SumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_FactoryNeedSumToNextLevelUpIsZero_SumSet()
        {
            var definition = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer();
            var factory = new Factory { NeedSumToNextLevelUp = 0, Level = 1, FactoryDefinition = definition, Customer = customer };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(3300, factory.NeedSumToNextLevelUp);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Calculate_FactorySumOnRDIsLessThanZero_SumsNotChanged(int sumOnRD)
        {
            var spentSum = 1;
            var description = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer { Sum = 1 };
            var factory = new Factory { NeedSumToNextLevelUp = 1, SumOnRD = sumOnRD, SpentSumToNextLevelUp = spentSum, FactoryDefinition = description, Customer = customer };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(0.9M, customer.Sum);
            Assert.AreEqual(spentSum, factory.SpentSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_PositiveFactorySumOnRDAndNotReady_SumsChagendAndLevelNeedSuToNextLevelNotChanged()
        {
            var definition = ReferenceData.FactoryDefinitions.First();
            const int level = 1;
            const int needSum = 1000;
            var customer = new Customer { Sum = 100 };
            var factory = new Factory { Level = level, NeedSumToNextLevelUp = needSum, SumOnRD = 10, SpentSumToNextLevelUp = 2, FactoryDefinition = definition, Customer = customer };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(81, customer.Sum);
            Assert.AreEqual(12, factory.SpentSumToNextLevelUp);
            Assert.AreEqual(level, factory.Level);
            Assert.AreEqual(1000, factory.NeedSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_ReadyForNextLevel_LevelIncreasedAndSumsChanged()
        {
            var definition = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer { Sum = 100 };
            var factory = new Factory { Level = 1, SumOnRD = 50, NeedSumToNextLevelUp = 10, SpentSumToNextLevelUp = 0, FactoryDefinition = definition, Customer = customer };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(2, factory.Level);
            Assert.AreEqual(45, customer.Sum);
            Assert.AreEqual(40, factory.SpentSumToNextLevelUp);
            Assert.AreEqual(7500, factory.NeedSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_FactoryWithoutSpecialTax_SumDecreasedByDefaultTax()
        {
            var description = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer { Sum = 100 };
            var factory = new Factory { FactoryDefinition = description, Customer = customer };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(90, customer.Sum);
        }

        [TestMethod]
        public void Calculate_AnyFactory_PerformanceSetWhileProducing()
        {
            var customer = new Customer();
            var factory = new Factory
            {
                Workers = 1,
                FactoryDefinition = new FactoryDefinition { GenerationLevel = 1, BaseWorkers = 1, ProductionType = new ProductionType() },
                Customer = customer
            };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(1, factory.Performance);
        }

        [TestMethod]
        public void Calculate_FactoryWithNotProducibleMaterial_MaterialRemoved()
        {
            var input = new Material { AmountPerDay = 1 };
            var material = new Material { AmountPerDay = 1, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input } } };
            var customer = new Customer();
            var factory = new Factory { FactoryDefinition = new FactoryDefinition { GenerationLevel = 1, ProductionType = new ProductionType() }, Customer = customer };
            factory.ProductionMaterials.Add(material);
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.IsFalse(factory.Stock.Any());
            Assert.IsFalse(factory.ProductionMaterials.Any());
        }

        [TestMethod]
        public void Calculate_FactoryWithProducibleMaterial_MaterialProducedAndMovedToStockAndInputMaterialDecreased()
        {
            var firstInput = new Material { AmountPerDay = 1 };
            var secondInput = new Material { AmountPerDay = 1 };
            var material = new Material
            {
                AmountPerDay = 1,
                InputMaterials = new List<MaterialOnStock> {
                    new MaterialOnStock { Material = firstInput, Amount = 2 },
                    new MaterialOnStock { Material = secondInput, Amount = 3 }
                }
            };
            var customer = new Customer();
            var factory = new Factory
            {
                FactoryDefinition = new FactoryDefinition { GenerationLevel = 1, ProductionType = new ProductionType(), BaseWorkers = 1 },
                Customer = customer,
                Workers = 1
            };
            factory.ProductionMaterials.Add(material);
            factory.Stock.Add(new MaterialOnStock { Material = firstInput, Amount = 3 });
            factory.Stock.Add(new MaterialOnStock { Material = secondInput, Amount = 5 });
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            var produced = factory.Stock.First(m => m.Material.Id == material.Id);
            var firstInputOnStock = factory.Stock.First(m => m.Material.Id == firstInput.Id);
            var secondInputOnStock = factory.Stock.First(m => m.Material.Id == secondInput.Id);
            Assert.IsTrue(factory.ProductionMaterials.Contains(material));
            Assert.AreEqual(3, factory.Stock.Count);
            Assert.AreSame(material, produced.Material);
            Assert.AreEqual(1, produced.Amount);
            Assert.AreEqual(1, firstInputOnStock.Amount);
            Assert.AreEqual(2, secondInputOnStock.Amount);
        }

        [TestMethod]
        public void Calculate_FactoryWithDifferentMaterials_NotProducibleRemovedAndProducibleReleased()
        {
            var input = new Material { AmountPerDay = 1 };
            var firstMaterial = new Material { AmountPerDay = 1, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 2 } } };
            var secondMaterial = new Material { AmountPerDay = 2, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 3 } } };
            var thirdMaterial = new Material { AmountPerDay = 3, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 100 } } };
            var fourthMaterial = new Material { AmountPerDay = 4, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 100 } } };
            var customer = new Customer();
            var factory = new Factory
            {
                Workers = 1,
                FactoryDefinition = new FactoryDefinition { GenerationLevel = 1, BaseWorkers = 1, ProductionType = new ProductionType() },
                Customer = customer
            };
            factory.ProductionMaterials.AddRange(new List<Material> { firstMaterial, secondMaterial, thirdMaterial, fourthMaterial });
            factory.Stock.Add(new MaterialOnStock { Material = input, Amount = 5 });
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            var firstProduced = factory.Stock.First(m => m.Material.Id == firstMaterial.Id);
            var secondProduced = factory.Stock.Find(m => m.Material.Id == secondMaterial.Id);
            var inputOnStock = factory.Stock.First(m => m.Material.Id == input.Id);
            Assert.IsTrue(factory.ProductionMaterials.Contains(firstMaterial));
            Assert.IsTrue(factory.ProductionMaterials.Contains(secondMaterial));
            Assert.IsFalse(factory.ProductionMaterials.Contains(thirdMaterial));
            Assert.IsFalse(factory.ProductionMaterials.Contains(fourthMaterial));
            Assert.AreEqual(3, factory.Stock.Count);
            Assert.AreEqual(2, factory.ProductionMaterials.Count);
            Assert.AreEqual(0.5M, firstProduced.Amount);
            Assert.AreEqual(1M, secondProduced.Amount);
            Assert.AreEqual(2.5M, inputOnStock.Amount);
        }

        [TestMethod]
        public void Calculate_FactoryWithProducibleMaterialAndStockTheSame_MaterialProducedAndStockCleaned()
        {
            var input = new Material { AmountPerDay = 1 };
            var material = new Material { AmountPerDay = 1, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 1 } } };
            var customer = new Customer();
            var factory = new Factory
            {
                Workers = 1,
                FactoryDefinition = new FactoryDefinition { ProductionType = new ProductionType(), BaseWorkers = 1, GenerationLevel = 1 },
                Customer = customer
            };
            factory.ProductionMaterials.Add(material);
            factory.Stock.Add(new MaterialOnStock { Material = input, Amount = 1 });
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            var produced = factory.Stock.First(m => m.Material.Id == material.Id);
            var inputOnStock = factory.Stock.FirstOrDefault(m => m.Material.Id == input.Id);
            Assert.AreEqual(1, factory.Stock.Count);
            Assert.IsTrue(factory.ProductionMaterials.Contains(material));
            Assert.AreEqual(material, produced.Material);
            Assert.IsNull(inputOnStock);
        }

        [TestMethod]
        public void Calculate_FactoryWithAnyMaterials_SumDecreasedOnSalary()
        {
            var customer = new Customer { Sum = 2000 };
            var factory = new Factory
            {
                Workers = 10,
                Customer = customer,
                FactoryDefinition = new FactoryDefinition { GenerationLevel = 10, ProductionType = new ProductionType(), BaseWorkers = 10 }
            };
            customer.Factories.Add(factory);
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(800, customer.Sum);
        }
    }
}