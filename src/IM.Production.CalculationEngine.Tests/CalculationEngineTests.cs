﻿using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            ReferenceData.InitReferences();
        }

        [TestMethod]
        public void Calculate_NoContractsWithGame_SumNotChanged()
        {
            var sum = 0;
            var customer = new Customer();
            var contract = new Contract(customer, null) {SourceFactory = new Factory()};
            _game.AddActivity(new CustomerNewContractChange(contract));
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(sum, customer.Sum);
        }

        [TestMethod]
        public void Calculate_ContractWithGameAndNoMaterialOnStock_NewMaterialAdded()
        {
            var material = ReferenceData.GetMaterialByKey(MaterialKeys.MetalRuda);
            var amount = 100;
            var customer = new Customer();
            _game.Customers.Add(customer);
            _game.AddActivity(new FinanceCustomerChange(customer, 10));
            var factory = Factory.CreateFactory(customer, null);
            var contract = new Contract(customer, new MaterialWithPrice { Material = material, Amount = amount }) { DestinationFactory = factory };
            _game.AddActivity(new CustomerNewContractChange(contract));

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
            var customer = new Customer();
            _game.AddActivity(new FinanceCustomerChange(customer, 10));
            var factory = Factory.CreateFactory(customer, null);
            var contract =
                new Contract(customer, new MaterialWithPrice {Amount = 100, Material = material})
                {
                    DestinationFactory = factory
                };
            var materialOnStock = new MaterialOnStock {Amount = 1, Material = material};
            _game.AddActivity(new FactoryAddMaterialToStockChange(factory, materialOnStock));
            _game.AddActivity(new CustomerNewContractChange(contract));
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(101, materialOnStock.Amount);
            Assert.AreEqual(9, customer.Sum);
        }

        [TestMethod]
        public void Calculate_SumForNextGenerationLevelIsZero_SetCorrectSum()
        {
            var customer = new Customer();
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(500, customer.SumToNextGenerationLevel);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Calculate_SumOnRDIsLowerThanZero_SumsNotChanged(int sumOnRD)
        {
            var sum = 1;
            var spent = 1;
            var customer = new Customer();
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, sum));
            _game.AddActivity(new CustomerSumOnRDChange(customer, sumOnRD));
            _game.AddActivity(new CustomerRDSpentChange(customer, spent, null));

            _calculationEngine.Calculate();

            Assert.AreEqual(sum, customer.Sum);
            Assert.AreEqual(spent, customer.SpentSumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_ReadyForNextGenerationLevel_GenerationLevelIncreasedAndSumsChanged()
        {
            var customer = new Customer();
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 10000));
            _game.AddActivity(new CustomerSumOnRDChange(customer, 6000));

            _calculationEngine.Calculate();

            Assert.AreEqual(2, customer.FactoryGenerationLevel);
            Assert.AreEqual(4000, customer.Sum);
            Assert.AreEqual(5500, customer.SpentSumToNextGenerationLevel);
            Assert.AreEqual(1500, customer.SumToNextGenerationLevel);
        }

        [TestMethod]
        public void Calculate_PositiveSumOnRDAndNotReady_SumsChangedAndGenerationLevelAndSumToNextNotChanged()
        {
            const int generationLevel = 1;
            const int sumToNext = 5000;
            var customer = new Customer();
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 10));
            _game.AddActivity(new CustomerSumOnRDChange(customer, 1));
            _game.AddActivity(new CustomerRDSpentChange(customer, 2, sumToNext));

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
            var factory = Factory.CreateFactory(customer, definition);

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _calculationEngine.Calculate();

            Assert.AreEqual(330, factory.NeedSumToNextLevelUp);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Calculate_FactorySumOnRDIsLessThanZero_SumsNotChanged(int sumOnRD)
        {
            var spentSum = 1;
            var description = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer();
            var factory = Factory.CreateFactory(null, description);

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 1));
            _game.AddActivity(new FactorySumOnRDChange(factory, sumOnRD));
            _game.AddActivity(new FactoryRDSpentChange(factory, spentSum, 1));

            _calculationEngine.Calculate();

            // -5 - налог на саму фабрику
            Assert.AreEqual(0.25m, customer.Sum);
            Assert.AreEqual(spentSum, factory.SpentSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_PositiveFactorySumOnRDAndNotReady_SumsChagendAndLevelNeedSuToNextLevelNotChanged()
        {
            var definition = ReferenceData.FactoryDefinitions.First();
            const int level = 1;
            const int needSum = 1000;
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer, definition);

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 100));
            _game.AddActivity(new FactorySumOnRDChange(factory, 10));
            _game.AddActivity(new FactoryRDSpentChange(factory, 2, needSum));

            _calculationEngine.Calculate();

            Assert.AreEqual(89.25m, customer.Sum);
            Assert.AreEqual(12, factory.SpentSumToNextLevelUp);
            Assert.AreEqual(level, factory.Level);
            Assert.AreEqual(1000, factory.NeedSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_ReadyForNextLevel_LevelIncreasedAndSumsChanged()
        {
            var definition = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer, definition);
            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 100));
            _game.AddActivity(new FactorySumOnRDChange(factory, 50));
            _game.AddActivity(new FactoryRDSpentChange(factory, 0, 10));

            _calculationEngine.Calculate();

            Assert.AreEqual(2, factory.Level);
            Assert.AreEqual(49.1m, customer.Sum);
            Assert.AreEqual(40, factory.SpentSumToNextLevelUp);
            Assert.AreEqual(750, factory.NeedSumToNextLevelUp);
        }

        [TestMethod]
        public void Calculate_FactoryWithoutSpecialTax_SumDecreasedByDefaultTax()
        {
            var description = ReferenceData.FactoryDefinitions.First();
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer, description);
            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 100));

            _calculationEngine.Calculate();

            Assert.AreEqual(99.25m, customer.Sum);
        }

        [TestMethod]
        public void Calculate_AnyFactory_PerformanceSetWhileProducing()
        {
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer, new FactoryDefinition {GenerationLevel = 1, BaseWorkers = 1, ProductionType = new ProductionType()});
            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FactoryWorkerCountChange(factory, 1));
            

            _calculationEngine.Calculate();

            Assert.AreEqual(1, factory.Performance);
        }

        [TestMethod]
        public void Calculate_FactoryWithNotProducibleMaterial_MaterialNotRemoved()
        {
            var input = new Material {AmountPerDay = 1};
            var material = new Material
            {
                AmountPerDay = 1,
                InputMaterials = new List<MaterialOnStock> {new MaterialOnStock {Material = input}}
            };
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer, new FactoryDefinition {GenerationLevel = 1, ProductionType = new ProductionType()});

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FactoryProductionMaterialChange(factory, new List<Material> { material }));

            _calculationEngine.Calculate();

            Assert.IsFalse(factory.Stock.Any());
            Assert.IsTrue(factory.ProductionMaterials.Any());
        }

        [TestMethod]
        public void Calculate_FactoryWithProducibleMaterial_MaterialProducedAndMovedToStockAndInputMaterialDecreased()
        {
            var firstInput = new Material {AmountPerDay = 1};
            var secondInput = new Material {AmountPerDay = 1};
            var material = new Material
            {
                AmountPerDay = 1,
                InputMaterials = new List<MaterialOnStock> {
                    new MaterialOnStock { Material = firstInput, Amount = 2 },
                    new MaterialOnStock { Material = secondInput, Amount = 3 }
                }
            };
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer,
                new FactoryDefinition {GenerationLevel = 1, ProductionType = new ProductionType(), BaseWorkers = 1});

            _game.AddActivity(new FactoryProductionMaterialChange(factory, new List<Material>{material}));
            _game.AddActivity(new FactoryAddMaterialToStockChange(factory, new MaterialOnStock {Material = firstInput, Amount = 3}));
            _game.AddActivity(new FactoryAddMaterialToStockChange(factory, new MaterialOnStock {Material = secondInput, Amount = 5}));

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FactoryWorkerCountChange(factory, 1));

            _calculationEngine.Calculate();

            var produced = factory.Stock.First(m => m.Material.Id == material.Id);
            var firstInputOnStock = factory.Stock.First(m => m.Material.Id == firstInput.Id);
            var secondInputOnStock = factory.Stock.First(m => m.Material.Id == secondInput.Id);
            Assert.IsTrue(factory.ProductionMaterials.Contains(material));
            Assert.AreEqual(3, factory.Stock.Count());
            Assert.AreSame(material, produced.Material);
            Assert.AreEqual(1, produced.Amount);
            Assert.AreEqual(1, firstInputOnStock.Amount);
            Assert.AreEqual(2, secondInputOnStock.Amount);
        }

        [TestMethod]
        public void Calculate_FactoryWithDifferentMaterials_NotProducibleNotRemovedAndProducibleReleased()
        {
            var input = new Material { AmountPerDay = 1, DisplayName = "входной материал"};
            var firstMaterial = new Material
            {
                AmountPerDay = 1,
                DisplayName = "выходной материал 1",
                InputMaterials = new List<MaterialOnStock> {new MaterialOnStock {Material = input, Amount = 2}}
            };
            var secondMaterial = new Material
            {
                AmountPerDay = 2,
                DisplayName = "выходной материал 2",
                InputMaterials = new List<MaterialOnStock> {new MaterialOnStock {Material = input, Amount = 3}}
            };
            var thirdMaterial = new Material
            {
                AmountPerDay = 3,
                DisplayName = "выходной материал 3",
                InputMaterials = new List<MaterialOnStock> {new MaterialOnStock {Material = input, Amount = 100}}
            };
            var fourthMaterial = new Material
            {
                AmountPerDay = 4,
                DisplayName = "выходной материал 4",
                InputMaterials = new List<MaterialOnStock> {new MaterialOnStock {Material = input, Amount = 100}}
            };

            var customer = new Customer();
            var factory = Factory.CreateFactory(customer,  new FactoryDefinition {GenerationLevel = 1, BaseWorkers = 1, ProductionType = new ProductionType()});

            _game.AddActivity(new FactoryProductionMaterialChange(factory,
                new List<Material> {firstMaterial, secondMaterial, thirdMaterial, fourthMaterial}));

            _game.AddActivity(new FactoryAddMaterialToStockChange(factory,
                new MaterialOnStock {Material = input, Amount = 5}));

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FactoryWorkerCountChange(factory, 1));

            _calculationEngine.Calculate();

            var firstProduced = factory.Stock.FirstOrDefault(m => m.Material.Id == firstMaterial.Id);
            var secondProduced = factory.Stock.FirstOrDefault(m => m.Material.Id == secondMaterial.Id);
            var thirdProduced = factory.Stock.FirstOrDefault(m => m.Material.Id == thirdMaterial.Id);
            var fourthProduced = factory.Stock.FirstOrDefault(m => m.Material.Id == fourthMaterial.Id);
            
            var inputOnStock = factory.Stock.First(m => m.Material.Id == input.Id);
            Assert.IsTrue(factory.ProductionMaterials.Contains(firstMaterial));
            Assert.IsTrue(factory.ProductionMaterials.Contains(secondMaterial));
            Assert.IsTrue(factory.ProductionMaterials.Contains(thirdMaterial));
            Assert.IsTrue(factory.ProductionMaterials.Contains(fourthMaterial));
            Assert.AreEqual(3, factory.Stock.Count());
            Assert.AreEqual(4, factory.ProductionMaterials.Count());
            Assert.AreEqual(0.5m, firstProduced.Amount);
            Assert.AreEqual(1, secondProduced.Amount);
            Assert.AreEqual(2.5m, inputOnStock.Amount);
            Assert.IsNull(thirdProduced);
            Assert.IsNull(fourthProduced);
        }

        [TestMethod]
        public void Calculate_FactoryWithProducibleMaterialAndStockTheSame_MaterialProducedAndStockCleaned()
        {
            var input = new Material { AmountPerDay = 1 };
            var material = new Material { AmountPerDay = 1, InputMaterials = new List<MaterialOnStock> { new MaterialOnStock { Material = input, Amount = 1 } } };
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer,
                new FactoryDefinition {ProductionType = new ProductionType(), BaseWorkers = 1, GenerationLevel = 1});
            _game.AddActivity(new FactoryProductionMaterialChange(factory, new List<Material>{material}));
            _game.AddActivity(new FactoryAddMaterialToStockChange(factory, new MaterialOnStock { Material = input, Amount = 1 }));
            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FactoryWorkerCountChange(factory, 1));

            _calculationEngine.Calculate();

            var produced = factory.Stock.First(m => m.Material.Id == material.Id);
            var inputOnStock = factory.Stock.FirstOrDefault(m => m.Material.Id == input.Id);
            Assert.AreEqual(1, factory.Stock.Count());
            Assert.IsTrue(factory.ProductionMaterials.Contains(material));
            Assert.AreEqual(material, produced.Material);
            Assert.IsNull(inputOnStock);
        }

        [TestMethod]
        public void Calculate_FactoryWithAnyMaterials_SumDecreasedOnSalary()
        {
            var customer = new Customer();
            var factory = Factory.CreateFactory(customer,
                new FactoryDefinition {GenerationLevel = 10, ProductionType = new ProductionType(), BaseWorkers = 10});

            _game.AddActivity(new CustomerBuyFactoryChange(customer, factory, 0));
            _game.Customers.Add(customer);

            _game.AddActivity(new FinanceCustomerChange(customer, 2000));
            _game.AddActivity(new FactoryWorkerCountChange(factory, 10));

            _calculationEngine.Calculate();

            Assert.AreEqual(1811, customer.Sum);
        }
    }
}