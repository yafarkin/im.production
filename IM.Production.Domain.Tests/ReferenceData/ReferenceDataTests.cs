﻿using Epam.ImitationGames.Production.Domain.Production;
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

            ReferenceData.Materials.Add(new Material
            {
                Key = "derevo",
                ProductionType = ReferenceData.GetProductionTypeByKey("derevo"),
                DisplayName = "Дерево"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 20m,
                Key = "brus",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("derevo"),
                        Amount = 1
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("derevo"),
                DisplayName = "Деревянный брус"
            });

            ReferenceData.Materials.Add(new Material
            {
                AmountPerDay = 1m,
                Key = "stul",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("brus"),
                        Amount = 4
                    },
                    new MaterialOnStock
                    {
                        Material = ReferenceData.GetMaterialByKey("gvozd"),
                        Amount = 15
                    }
                },
                ProductionType = ReferenceData.GetProductionTypeByKey("derevo"),
                DisplayName = "Деревянный стул"
            });

            ReferenceData.Supply.Materials = new List<MaterialWithPrice>
            {
                new MaterialWithPrice {Material = ReferenceData.GetMaterialByKey("metall_ruda"), SellPrice = 0.02m},
                new MaterialWithPrice {Material = ReferenceData.GetMaterialByKey("derevo"), SellPrice = 5}
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

            m = ReferenceData.GetMaterialByKey("gvozd");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(0.025m, p);

            m = ReferenceData.GetMaterialByKey("derevo");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(5, p);

            m = ReferenceData.GetMaterialByKey("brus");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(0.25m, p);

            m = ReferenceData.GetMaterialByKey("stul");
            p = ReferenceData.CalculateMaterialCostPrice(m);
            Assert.AreEqual(1.375m, p);
        }

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

            m = ReferenceData.GetMaterialByKey("gvozd");
            ReferenceData.CalculateMaterialInDetails(m, out p, out sm);
            Assert.AreEqual(0.025m, p);
            Assert.AreEqual(3, sm.Count);
            Assert.AreEqual("metall_zelezo_list", sm[0].Material.Key);
            Assert.AreEqual(0.0005m, sm[0].Amount);
            Assert.AreEqual("metall_zelezo", sm[1].Material.Key);
            Assert.AreEqual(0.0025m, sm[1].Amount);
            Assert.AreEqual("metall_ruda", sm[2].Material.Key);
            Assert.AreEqual(1.25m, sm[2].Amount);

            m = ReferenceData.GetMaterialByKey("stul");
            ReferenceData.CalculateMaterialInDetails(m, out p, out sm);
            Assert.AreEqual(1.375m, p);
            Assert.AreEqual(6, sm.Count);
            Assert.AreEqual("brus", sm[0].Material.Key);
            Assert.AreEqual(4, sm[0].Amount);
            Assert.AreEqual("derevo", sm[1].Material.Key);
            Assert.AreEqual(0.2m, sm[1].Amount);
            Assert.AreEqual("gvozd", sm[2].Material.Key);
            Assert.AreEqual(15, sm[2].Amount);
            Assert.AreEqual("metall_zelezo_list", sm[3].Material.Key);
            Assert.AreEqual(0.0075m, sm[3].Amount);
            Assert.AreEqual("metall_zelezo", sm[4].Material.Key);
            Assert.AreEqual(0.0375m, sm[4].Amount);
            Assert.AreEqual("metall_ruda", sm[5].Material.Key);
            Assert.AreEqual(18.75m, sm[5].Amount);
        }

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

            var demand = ReferenceData.Demand.Materials.First();
            Assert.AreSame(material, demand.Material);
            Assert.AreEqual(0.022M, demand.SellPrice);
        }

        [TestMethod]
        public void UpdateGameDemand_MaterialExists_MaterialNotAddedAdditionally()
        {
            var material = ReferenceData.GetMaterialByKey("metal_ruda");
            var factories = new List<Factory> { new Factory { ProductionMaterials = new List<Material> { material } } };
            ReferenceData.Demand.Materials.Add(new MaterialWithPrice { Material = material });

            ReferenceData.UpdateGameDemand(factories);

            Assert.AreEqual(1, ReferenceData.Demand.Materials.Count);
            Assert.AreEqual(material, ReferenceData.Demand.Materials.First());
        }
    }
}
