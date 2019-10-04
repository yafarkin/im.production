using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Common.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SantaHelpers;

namespace dev
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void CreateExampleMaterialCollection()
        {
            var fileName = @"c:\tmp\materials.json";

            var wood = new Material { DisplayName = "Дерево", AmountPerDay = 1};
            var nail = new Material { DisplayName = "Гвоздь", AmountPerDay = 100};

            var chair = new Material
            {
                DisplayName = "Стул",
                InputMaterials = new List<MaterialOnStock>
                {
                    new MaterialOnStock {Material = wood, Amount = 3},
                    new MaterialOnStock {Material = nail, Amount = 10}
                }
            };

            var materials = new List<Material> {wood, nail, chair};

            FileHelper.Save(fileName, materials);
        }
    }
}
