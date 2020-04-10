using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using IM.Production.CalculationEngine;
using IM.Production.WebApp.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IM.Production.WebApp.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class FakeGameFactory
    {
        public static Game CreateGame(IServiceProvider serviceProvider)
        {
            var game = new Game();
            var logic = new Logic(game);
            var rand = new Random();
            var materialList = new List<MaterialWithPrice>();

            var gameOptions = serviceProvider.GetRequiredService<IOptions<GameOptions>>();
            game.TotalGameDays = gameOptions.Value.TotalDays;

            #region CreateMaterial
            for (var i = 0; i < 100; i++)
            {
                var material = new MaterialWithPrice
                {
                    Id = new Guid(),
                    Amount = rand.Next(10, 100_000),
                    SellPrice = rand.Next(250, 50_000)
                };
                materialList.Add(material);
            }
            #endregion

            #region CreateCustomers
            var customerList = new List<Customer>();

            #region Add customer game
            {
                var customer = logic.AddNewCustomer("Game", "GamePassword", "GameName");
                customer.DisplayName = "Game";
                customerList.Add(customer);
            }
            #endregion

            for (var c = 0; c < 30; c++)
            {
                var productionType = new ProductionType
                {
                    Key = "key"
                };
                var randNumber = rand.Next(0, 100);
                productionType.DisplayName = randNumber < 30 ? "Metallurgical" : randNumber >= 30 && randNumber < 60 ? "Oil and gas and chemical" : "Electronic";

                var customer = logic.AddNewCustomer("CustomerLogin" + c, "CustomerPassword" + c,
                    "CustomerName" + c);
                customer.DisplayName = "Customer" + c + (char)(rand.Next(0, 100) < 50 ? rand.Next('A', 'Z') : rand.Next('a', 'z'));
                customerList.Add(customer);
            }
            #endregion

            static Factory GetFirstFactory(IEnumerable<Factory> factories)
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
                        var materialWithPrice = new MaterialWithPrice
                        {
                            SellPrice = 1200
                        };

                        var contract = new Contract(customer, materialWithPrice);
                        var bfo = new BankCredit(customer, 5_000_000)
                        {
                            Percent = 0.0M
                        };
                        logic.TakeDebitOrCredit(customer, bfo);

                        var factoryDefinition = new FactoryDefinition
                        {
                            BaseWorkers = 50,
                            ProductionType = ReferenceData.GetProductionTypeByKey(customer.ProductionType.Key),
                            GenerationLevel = 1
                        };

                        var listOfProductionMaterials = new Dictionary<int, List<Material>>
                        {
                            { 1, new List<Material>() { material } }
                        };
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
                                    (int?)(((r * materialNumber) + (c * DateTime.Now.Ticks) + 1) / (DateTime.Now.Second + 1) % 10_000_000);
                                contract.TotalCountCompleted = 50;
                                contract.TillDate =
                                    (int?)(DateTime.Now.Ticks % 10_000_000);
                                contract.TotalSumm =
                                    (decimal)((contract.TillCount * contract.TillDate) + 1) % 10_000_000;

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
                                (int?)(((r * materialNumber) + (c * DateTime.Now.Ticks) + 1) / (DateTime.Now.Second + 1) % 10_000_000);
                            contract.TotalCountCompleted = 50;
                            contract.TillDate =
                                (int?)(DateTime.Now.Ticks % 10_000_000);
                            contract.TotalSumm =
                                (decimal)((contract.TillCount * contract.TillDate) + 1) % 10_000_000;
                            contract.DestinationFactory = c >= 1 ? GetFirstFactory(customerList[c - 1].Factories) : new Factory();

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
