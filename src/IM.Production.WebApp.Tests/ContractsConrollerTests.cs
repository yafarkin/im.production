using IM.Production.WebApp.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.WebApp.Tests
{
    [TestClass]
    public class ContractsConrollerTests
    {
        [TestMethod]
        public void GetAllContracts()
        {
            var contractsController = new ContractsController();
            var allContracts = contractsController.GetContracts();
            Assert.IsNotNull(allContracts);
        }
    }
}
