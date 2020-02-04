using IM.Production.WebApp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.WebApp.Tests
{
    [TestClass]
    public class ContractControllerTests
    {
        [TestMethod]
        public void GetAllContracts()
        {
            var contractsController = new ContractsController();
            var allContracts = contractsController.GetAllContracts();
            Assert.IsNotNull(allContracts);
        }

    }
}
