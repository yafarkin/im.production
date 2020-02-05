using System.Collections.Generic;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
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

            #region CreateCustomers
            var customerList = new List<Customer>();

            #region Add customer game
            {
                var productionType = new ProductionType();
                productionType.Key = "GameProductionTypeKey";
                productionType.DisplayName = "GameProductionTypeDisplayName";
                var customer = logic.AddCustomer("Game", "GamePassword", "GameName", productionType);
                customerList.Add(customer);
            }
            #endregion

            for (var c = 0; c < customersCount; c++)
            {
                var productionType = new ProductionType();
                productionType.Key = "key";
                productionType.DisplayName = "DisplayName";
                var customer = logic.AddCustomer("CustomerLogin" + c, "CustomerPassword" + c,
                    "CustomerName" + c, productionType);
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
                var contractsNumber = rand.Next(3, 10);
                for (var r = 0; r < contractsNumber; r++)
                {
                    var materialNumber = rand.Next(1, 99);
                    if (materialNumber > 0 && materialNumber < materialList.Count)
                    {
                        #region GiveMoneyAndFabrics
                        var material = materialList[materialNumber];
                        var contract = new Contract(customer, material);
                        var bfo = new BankCredit(customer, 5_000_000);
                        bfo.Percent = 0.0M;
                        logic.TakeDebitOrCredit(customer, bfo);

                        var factoryDefinition = new FactoryDefinition();
                        factoryDefinition.BaseWorkers = 50;
                        factoryDefinition.ProductionType = new ProductionType();
                        factoryDefinition.ProductionType.Id = customer.ProductionType.Id;
                        factoryDefinition.ProductionType.Key = "key";
                        factoryDefinition.ProductionType.DisplayName = "display name";
                        factoryDefinition.GenerationLevel = 1;
                        var d = new Dictionary<int, List<Material>>();
                        factoryDefinition.CanProductionMaterials = d;
                        logic.BuyFactoryFromGame(customer, factoryDefinition, 30);
                        logic.BuyFactoryFromGame(customer, factoryDefinition, 30);
                        #endregion

                        #region CreateContract
                        contract.SourceFactory = GetFirstFactory(customer.Factories);
                        contract.TillCount =
                            (int?)((r * materialNumber + c * System.DateTime.Now.Ticks + 1) / (System.DateTime.Now.Second + 1) % 10_000_000);
                        contract.TillDate =
                            (int?)(System.DateTime.Now.Ticks % 10_000_000);
                        contract.TotalSumm =
                            (decimal)(contract.TillCount * contract.TillDate + 1) % 10_000_000;
                        contract.DestinationFactory = (c >= 1) ? GetFirstFactory(customerList[c - 1].Factories) : new Factory();

                        if (contract?.DestinationFactory != null && contract?.SourceFactory != null)
                        {
                            logic.AddContract(contract);
                        }
                        #endregion

                    }
                }
            }
            return game;
        }
    }
}
