using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.ReferenceData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace IM.Production.Domain.Tests
{
    [TestClass]
    public class ReferenceDataSerializer
    {
        [TestMethod]
        public void Serialize()
        {

            var Data = ReferenceData.GetData();
            var path = @"..\..\..\..\..\im.production\IM.Production.Domain\InitialData.json";
            var stringData = JsonConvert.SerializeObject(Data);
            File.WriteAllText(path, stringData);
        }
    }
}
