using AutoMapper;
using IM.Production.WebApp.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.WebApp.Tests.Configuration
{
    [TestClass]
    public class MappingProfileTests
    {
        [TestMethod]
        public void Constructor_Default_MapsRegistered()
        {
            var configuration = new MapperConfiguration(e => e.AddProfile<MappingProfile>());

            configuration.AssertConfigurationIsValid();
        }
    }
}
