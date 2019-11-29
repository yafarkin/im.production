using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace IM.Production.Domain.Tests
{
    [TestClass]
    public class ReferenceDataTests
    {
        [TestInitialize]
        public void Init()
        {
            ReferenceData.Data.Materials.Clear();

            ReferenceData.Data.Materials.Add(new Material
            {
                Key = "metall_ruda",
                ProductionType = ReferenceData.GetProductionTypeByKey("metall"),
                DisplayName = "Металлосодержащая руда"
            });

            ReferenceData.Data.Materials.Add(new Material
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

            ReferenceData.Data.Materials.Add(new Material
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

            ReferenceData.Data.Supply.Materials = new List<MaterialWithPrice>
            {
                new MaterialWithPrice {Material = ReferenceData.GetMaterialByKey("metall_ruda"), SellPrice = 0.02m}
            };
        }

        [TestMethod]
        public void CheckCalcPrices_Test()
        {
            var m = ReferenceData.GetMaterialByKey("metall_ruda");
            var p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(0.02m, p);

            m = ReferenceData.GetMaterialByKey("metall_zelezo");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(10, p);

            m = ReferenceData.GetMaterialByKey("metall_zelezo_list");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(50, p);            
        }

        
        //TODO
        /*
        [TestMethod]
        public void CheckMaterialInDetails_Test()
        {
                var m = ReferenceData.GetMaterialByKey("metall_ruda");
                ReferenceData.CalculateMaterialInDetails(m, out var p, out var sm);
                Assert.AreEqual(0.02m, p);
                Assert.AreEqual(0, sm.Count);

                m = ReferenceData.GetMaterialByKey("metall_zelezo");
                ReferenceData.CalculateMaterialInDetails(m, out p, out sm);
                Assert.AreEqual(10, p);
                Assert.AreEqual(1, sm.Count);
                Assert.AreEqual("metall_ruda", sm[0].Material.Key);
                Assert.AreEqual(500, sm[0].Amount);

                m = ReferenceData.GetMaterialByKey("metall_zelezo");
                ReferenceData.CalculateMaterialInDetails(m, out p, out sm, 2);
                Assert.AreEqual(20, p);
                Assert.AreEqual(1, sm.Count);
                Assert.AreEqual("metall_ruda", sm[0].Material.Key);
                Assert.AreEqual(1000, sm[0].Amount);

                m = ReferenceData.GetMaterialByKey("metall_zelezo_list");
                ReferenceData.CalculateMaterialInDetails(m, out p, out sm);
                Assert.AreEqual(50, p);
                Assert.AreEqual(2, sm.Count);
                Assert.AreEqual("metall_zelezo", sm[0].Material.Key);
                Assert.AreEqual(5, sm[0].Amount);
                Assert.AreEqual("metall_ruda", sm[1].Material.Key);
                Assert.AreEqual(2500, sm[1].Amount);            
            }
            */

        [TestMethod]
        public void CalculateFactoryExtraChargePercent_DefaultGeneration_DefaultPercentReturned()
        {
            var factory = new Factory { FactoryDefinition = new FactoryDefinition { GenerationLevel = 1 } };
            var factories = new List<Factory>
            {
                new Factory{FactoryDefinition = new FactoryDefinition{GenerationLevel = 2}},
                factory
            };

            var percent = ReferenceData.CalculateFactoryExtraChargePercent(factory, factories);

            Assert.AreEqual(0.1M, percent);
        }

        [TestMethod]
        public void CalculateFactoryExtraChargePercent_NotDefaultNorNotDeveloped_DefaultPercentReturned()
        {
            var factory = new Factory { FactoryDefinition = new FactoryDefinition { GenerationLevel = 2 } };
            var factories = new List<Factory>
            {
                new Factory{FactoryDefinition = new FactoryDefinition{GenerationLevel = 3}},
                factory
            };

            var percent = ReferenceData.CalculateFactoryExtraChargePercent(factory, factories);

            Assert.AreEqual(0.1M, percent);
        }

        [TestMethod]
        public void CalculateFactoryExtraChargePercent_Developed_AdvancedPercentReturned()
        {
            var factory = new Factory { FactoryDefinition = new FactoryDefinition { GenerationLevel = 3 } };
            var factories = new List<Factory>
            {
                new Factory{FactoryDefinition = new FactoryDefinition{GenerationLevel = 2}},
                factory
            };

            var percent = ReferenceData.CalculateFactoryExtraChargePercent(factory, factories);

            Assert.AreEqual(0.5M, percent);
        }

        [TestMethod]
        public void UpdateGameDemand_NotExistingMaterial_MaterialAdded()
        {
            var material = ReferenceData.GetMaterialByKey("metall_ruda");
            var factory = new Factory
            {
                FactoryDefinition = new FactoryDefinition(),
                ProductionMaterials = new List<Material> { material }
            };
            var factories = new List<Factory> { factory };

            ReferenceData.UpdateGameDemand(factories);

            var demand = ReferenceData.Data.Demand.Materials.First();
            Assert.AreSame(material, demand.Material);
            Assert.AreEqual(0.022M, demand.SellPrice);
        }
    }
}
