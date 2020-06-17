using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
            var customerList = new List<Customer>();
            var metalProductionTypeId = ReferenceData.ProductionTypes[0].Id;

            Factory GetFactoryForContract(Guid customerId)
            {
                var customers = customerList.Where(customer => customer.ProductionType.Id == metalProductionTypeId && customer.Id != customerId).ToArray();
                var customersCounts = customers.Select(customer => customer.Contracts.Count()).ToArray();
                var min = customersCounts[0];
                var minIndex = 0;

                for (var i = 1; i < customersCounts.Length; i++)
                {
                    var count = customersCounts[i];
                    if (count < min)
                    {
                        min = count;
                        minIndex = i;
                    }
                }

                return customers[minIndex].Factories.First();
            }

            #region CreateCustomers

            for (var c = 0; c < customersCount; c++)
            {
                var randNumber = rand.Next(0, 3);
                var customer = logic.AddNewCustomer("CustomerLogin" + c, "CustomerPassword" + c, "CustomerName" + c);
                customer.DisplayName = "Customer" + c;
                customerList.Add(customer);
            }

            #endregion

            #region Give Money And Buy Factories

            for (var c = 0; c < customerList.Count; c++)
            {
                var customer = customerList[c];

                if (customer.ProductionType.Id != metalProductionTypeId)
                {
                    continue;
                }

                var backCreadit = new BankCredit(customer, 5_000_000);
                backCreadit.Percent = 0.0M;
                logic.TakeDebitOrCredit(customer, backCreadit);

                var defenitions = ReferenceData.GetAvailFactoryDefenitions(customer, false);
                foreach (var define in defenitions)
                {
                    logic.BuyFactoryFromGame(customer, define, 30);
                }

            }

            #endregion

            #region Create Contracts For Each Customer

            for (var c = 0; c < customerList.Count; c++)
            {
                var customer = customerList[c];
                var contractsNumber = rand.Next(3, 7);
                for (var r = 0; r < contractsNumber; r++)
                {
                    var materialWithPrice = new MaterialWithPrice
                    {
                        Material = ReferenceData.GetMaterialByKey("metall_ruda"),
                        SellPrice = 0.01m,
                        Amount = 10000
                    };

                    var flag = 0;
                    CreateContract:
                    var contract = new Contract(customer, materialWithPrice);
                    foreach (var factory in customer.Factories)
                    {
                        contract.TotalCountCompleted = 0;
                        contract.TillDate = (int?)(System.DateTime.Now.Ticks % 10_000_000);
                        contract.TotalSumm = materialWithPrice.SellPrice * materialWithPrice.Amount;
                        contract.SourceFactory = factory;
                        contract.DestinationFactory = GetFactoryForContract(customer.Id);

                        if (contract?.DestinationFactory != null && contract?.SourceFactory != null)
                        {
                            logic.AddContract(contract);
                        }
                    }

                    if (c == 0 && flag < 5 || c != 0 && flag < 3)
                    {
                        ++flag;
                        goto CreateContract;
                    }
                }
            }

            #endregion

            foreach (var customer in customerList)
            {
                var contract = customer.Contracts.FirstOrDefault();
                if (contract != null)
                {
                    logic.CloseContract(contract);
                }
            }

            return game;
        }
    }
}