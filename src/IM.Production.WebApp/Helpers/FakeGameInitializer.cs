using System.Collections.Generic;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using IM.Production.CalculationEngine;

namespace IM.Production.WebApp.Helpers
{
    public class FakeGameInitializer
    {
        public static Game CreateGame(int customersCount)
        {
            var game = new Game();
            var logic = new Logic(game);
            var rand = new System.Random();
            var materialList = new List<MaterialWithPrice>();

            #region CreateMaterial
            for (var i = 0; i < 100; i++)
            {
                var material = new MaterialWithPrice();
                material.Id = new System.Guid();
                material.Amount = rand.Next(10, 100_000);
                material.SellPrice = rand.Next(250, 50_000);
                materialList.Add(material);
            }
            #endregion

            var canProduceMaterials = new List<Material>();
            canProduceMaterials.Add(new Material());
            canProduceMaterials.Add(new Material());
            canProduceMaterials.Add(new Material());

            #region CreateCustomers
            var customerList = new List<Customer>();

            #region Add customer game
            {
                var productionType = new ProductionType();
                productionType.Key = "GameProductionTypeKey";
                productionType.DisplayName = "GameProductionTypeDisplayName";
                var customer = logic.AddCustomer("Game", "GamePassword", "GameName");
                customer.DisplayName = "Game";
                customerList.Add(customer);
            }
            #endregion

            /*
                ProductionType:
                * Metallurgical industry
                * Oil and gas and chemical industry
                * Electronic industry
             */

            for (var c = 0; c < customersCount; c++)
            {
                var productionType = new ProductionType();
                productionType.Key = "key";
                var randNumber = rand.Next(0, 100);
                if (randNumber < 30)
                {
                    productionType.DisplayName = "Metallurgical";
                }
                else if (randNumber >= 30 && randNumber < 60)
                {
                    productionType.DisplayName = "Oil and gas and chemical";
                }
                else
                {
                    productionType.DisplayName = "Electronic";
                }

                var customer = logic.AddCustomer("CustomerLogin" + c, "CustomerPassword" + c,
                    "CustomerName" + c);
                customer.DisplayName = "Customer" + c + (char)((rand.Next(0, 100) < 50) ? rand.Next('A', 'Z') : rand.Next('a', 'z')); 
                customerList.Add(customer);
            }
            #endregion

            Factory GetFirstFactory(IEnumerable<Factory> factories)
            {
                foreach (var el in factories)
                {
                    if (el != null)
                    {
                        return el;
                    }
                }
                return new Factory();
            }

            for (var c = 0; c < customerList.Count; c++)
            {
                var customer = customerList[c];
                var contractsNumber = rand.Next(3, 7);
                for (var r = 0; r < contractsNumber; r++)
                {
                    var materialNumber = rand.Next(1, 99);
                    if (materialNumber > 0 && materialNumber < materialList.Count)
                    {
                        #region GiveMoneyAndFabrics
                        var material = ReferenceData.GetMaterialByKey("metall_zelezo");
                        var materialOnStock = new MaterialOnStock();
                        materialOnStock.Amount = 150_000;
                        materialOnStock.Material = material;
                        var materialWithPrice = new MaterialWithPrice();
                        materialWithPrice.SellPrice = 1200;

                        var contract = new Contract(customer, materialWithPrice);
                        var bfo = new BankCredit(customer, 5_000_000);
                        bfo.Percent = 0.0M;
                        logic.TakeDebitOrCredit(customer, bfo);

                        var factoryDefinition = new FactoryDefinition();
                        factoryDefinition.BaseWorkers = 50;
                        factoryDefinition.ProductionType = ReferenceData.GetProductionTypeByKey(customer.ProductionType.Key);
                        factoryDefinition.GenerationLevel = 1;
                        
                        var listOfProductionMaterials = new Dictionary<int, List<Material>>();
                        listOfProductionMaterials.Add(1, new List<Material>() { material });
                        factoryDefinition.CanProductionMaterials = listOfProductionMaterials;
                        logic.BuyFactoryFromGame(customer, factoryDefinition, 30);
                        #endregion
                        
                        #region CreateContract

                        if (c > 0)
                        {
                            foreach (var factory in customer.Factories)
                            {
                                contract.SourceFactory = factory;
                                contract.TillCount =
                                    (int?)((r * materialNumber + c * System.DateTime.Now.Ticks + 1) / (System.DateTime.Now.Second + 1) % 10_000_000);
                                contract.TotalCountCompleted = 50;
                                contract.TillDate =
                                    (int?)(System.DateTime.Now.Ticks % 10_000_000);
                                contract.TotalSumm =
                                    (decimal)(contract.TillCount * contract.TillDate + 1) % 10_000_000;

                                contract.DestinationFactory = GetFirstFactory(customerList[c - 1].Factories);

                                if (contract?.DestinationFactory != null && contract?.SourceFactory != null)
                                {
                                    logic.AddContract(contract);
                                }
                            }
                        }
                        else
                        {
                            contract.SourceFactory = GetFirstFactory(customer.Factories);
                            contract.TillCount =
                                (int?)((r * materialNumber + c * System.DateTime.Now.Ticks + 1) / (System.DateTime.Now.Second + 1) % 10_000_000);
                            contract.TotalCountCompleted = 50;
                            contract.TillDate =
                                (int?)(System.DateTime.Now.Ticks % 10_000_000);
                            contract.TotalSumm =
                                (decimal)(contract.TillCount * contract.TillDate + 1) % 10_000_000;
                            contract.DestinationFactory = (c >= 1) ? GetFirstFactory(customerList[c - 1].Factories) : new Factory();

                            if (contract?.DestinationFactory != null && contract?.SourceFactory != null)
                            {
                                logic.AddContract(contract);
                            }
                        }
                        #endregion

                    }
                }
            }
            return game;
        }
    }
}
