using AutoMapper;
using IM.Production.WebApp.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IM.Production.WebApp.Tests.Infrastructure
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
