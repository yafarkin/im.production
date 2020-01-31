using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Production;
using IM.Production.CalculationEngine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IM.Production.WebApp.Controllers
{
    public class A
    {
        public int F1 { get; set; }
    }

    public class B: A
    {
        public IEnumerable<string> Name { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ContractsController : ControllerBase
    {

        [HttpGet]
        [Route("GetAllContracts")]
        public Customer GetAllContracts()
        {
            var gameInstance = Startup.GameInstance;
            var customerContracts = new List<Contract>();
            foreach (var customer in gameInstance.Customers)
            {
                customerContracts.AddRange(customer.Contracts);
            }
            var contractsArray = customerContracts.ToArray();

            var material = new MaterialWithPrice();
            material.Amount = 100_000;
            material.SellPrice = 250;
            var productionType = new ProductionType();
            productionType.Key = "key";
            productionType.DisplayName = "DisplayName";

            var contract = new Contract(new Customer(), material);
            contract.SourceFactory = new Factory();
            contract.DestinationFactory = new Factory();
#if FALSE
            var customer = logic.AddCustomer("CustomerLogin" + c, "CustomerPassword" + c, "CustomerName" + c, productionType);
            var contract = new Contract(customer, material);
            contract.SourceFactory = new Factory();
            contract.DestinationFactory = new Factory();
            logic.AddContract(contract);
#endif
            contract = contractsArray[12];
            {
                var customer = contract.DestinationFactory.Customer;
            }

#if FALSE
            +
            Factory
            boolean
            int?
            decimal
            Material
            MaterialLogistic
            MaterialWithPrice
            TaxFactoryChange

            -
            Customer

#endif
            
            return contract.Customer;
        }

    }
}