using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.CalculationEngine.Tests
{
    [TestClass]
    public class SmokeTests
    {
        public Game Game;
        public CalculationEngine Engine;
        public Logic Logic;

        [TestInitialize]
        public void Init()
        {
            Game = new Game();
            Engine = new CalculationEngine(Game);
            Logic = new Logic(Game);

            InitMaterials();
            InitFactoryDefinition();
        }

        #region init reference data

        private void InitFactoryDefinition()
        {
            ReferenceData.FactoryDefinitions.Clear();

            // металлургия
            // руда -> железный слиток -> лист -> проволока -> электронный блок=блок управления
            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 10,
                GenerationLevel = 1,
                Name = "Добыча металлической руды",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_ruda")}},
                    {2, new List<Material> {ReferenceData.GetMaterialByKey("metall_med_ruda")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 8,
                GenerationLevel = 2,
                Name = "Производство железной руды",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 5,
                GenerationLevel = 3,
                Name = "Производство металлических листов",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("metall_zelezo_list")}},
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 3,
                GenerationLevel = 4,
                Name = "Производство проволоки",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("provoloka")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                BaseWorkers = 3,
                GenerationLevel = 5,
                Name = "Производство блоков управления",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("blok_upravlenia")}}
                }
            });

            // электроника
            // кремниевая порода -> кремний -> проволока+кремний=печатная плата -> электронный блок -> компьютер
            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 10,
                GenerationLevel = 1,
                Name = "Добыча кремниевой породы",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii_ruda")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 3,
                GenerationLevel = 2,
                Name = "Производство кремния",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electronic_kremnii")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 12,
                GenerationLevel = 3,
                Name = "Производство печатной платы",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("plata")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 4,
                GenerationLevel = 4,
                Name = "Производство электронных блоков",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("electro_blok")}}
                }
            });

            ReferenceData.FactoryDefinitions.Add(new FactoryDefinition
            {
                ProductionType = ReferenceData.GetProductionTypeByKey("electronic"),
                BaseWorkers = 4,
                GenerationLevel = 5,
                Name = "Производство компьютеров",
                CanProductionMaterials = new Dictionary<int, List<Material>>
                {
                    {1, new List<Material> {ReferenceData.GetMaterialByKey("computer")}}
                }
            });

        }

        private void InitMaterials()
        {
            ReferenceData.Materials.Clear();

            ReferenceData.Materials.Add(new Material
            {
                Key = "metall_ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Металлосодержащая руда"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 2m,
                Key = "metall_zelezo",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_ruda"),
                        Amount = 1000
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Железная руда"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "metall_zelezo_list",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_zelezo"),
                        Amount = 5
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Стальной лист"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 2000m,
                Key = "gvozd",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_zelezo_list"),
                        Amount = 1
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Гвоздь"
            });
        }

        #endregion

        protected void RunCycles(int cycles = 1)
        {
            for (var i = 0; i < cycles; i++)
            {
                Engine.Calculate();
            }
        }

        [TestMethod]
        public void GameFlowSmokeTest()
        {
            // дымовой тест, проверяющий все основные функции игры
            // добавляем две команды
            // первая команда берёт кредит
            // обе команды покупают по фабрике
            // обе команды откладывают деньги на апгрейд фабрики и на следующий уровень фабрик
            // обе команды начинают производить первичные материалы
            
            var c1 = Logic.AddCustomer("c1", "1", "команда 1", ReferenceData.GetProductionTypeByKey("metall"));
            var c2 = Logic.AddCustomer("c2", "2", "команда 2", ReferenceData.GetProductionTypeByKey("electronic"));
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreEqual(0, c1.Contracts.Count);
            Assert.AreEqual(0, c1.BankFinanceOperations.Count);
            Assert.AreEqual(0, c1.BankFinanceActions.Count);
            Assert.AreEqual(0, c1.Factories.Count);
            Assert.AreEqual(1, c1.FactoryGenerationLevel);
            Assert.AreEqual(0, c2.Contracts.Count);
            Assert.AreEqual(0, c2.BankFinanceOperations.Count);
            Assert.AreEqual(0, c2.BankFinanceActions.Count);
            Assert.AreEqual(0, c2.Factories.Count);
            Assert.AreEqual(1, c2.FactoryGenerationLevel);

            Logic.TakeDebitOrCredit(c1, new BankCredit(Game.Time, c1, 1000000));
            Assert.AreEqual(ReferenceData.InitialCustomerBalance + 1000000, c1.Sum);
            Assert.AreEqual(ReferenceData.InitialCustomerBalance, c2.Sum);

            Assert.AreEqual(1, c1.BankFinanceOperations.Count);
            Assert.AreEqual(1, c1.BankFinanceActions.Count);
            Assert.AreEqual(0, c2.BankFinanceOperations.Count);
            Assert.AreEqual(0, c2.BankFinanceActions.Count);

            var f1 = Logic.BuyFactoryFromGame(c1, ReferenceData.GetAvailFactoryDefenitions(c1).First());
            var f2 = Logic.BuyFactoryFromGame(c2, ReferenceData.GetAvailFactoryDefenitions(c2).First());
            Assert.IsNotNull(f1);
            Assert.IsNotNull(f2);
            Assert.AreEqual(1, c1.Factories.Count);
            Assert.AreEqual(1, c2.Factories.Count);

            Assert.AreEqual(ReferenceData.InitialCustomerBalance + 990000, c1.Sum);
            Assert.AreEqual(ReferenceData.InitialCustomerBalance - 10000, c2.Sum);

            Logic.UpdateFactorySettings(f1, null, 1000);
            Logic.UpdateFactorySettings(f2, null, 500);

            Logic.UpdateCustomerSettings(c1, 20000);
            Logic.UpdateCustomerSettings(c2, 10000);

            RunCycles();

        }
    }
}