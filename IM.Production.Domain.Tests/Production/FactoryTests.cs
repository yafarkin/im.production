﻿using Epam.ImitationGames.Production.Domain.Production;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace IM.Production.Domain.Tests.Production
{
    [TestClass]
    public class FactoryTests
    {
        [TestMethod]
        public void Constructor_Default_InitializedStock()
        {
            var factory = new Factory();

            Assert.IsNotNull(factory.Stock);
            Assert.IsFalse(factory.Stock.Any());
        }

        [TestMethod]
        public void Constructor_Default_InitializedDefaultLevel()
        {
            var factory = new Factory();

            Assert.AreEqual(1, factory.Level);
        }

        [DataTestMethod]
        [DataRow(1, 2, true)]
        [DataRow(1, 1, true)]
        [DataRow(2, 1, false)]
        public void ReadyForNextLevel_CorrectSums_ReadyOrNotReturned(int needSum, int spentSum, bool expected)
        {
            var factory = new Factory { NeedSumToNextLevelUp = needSum, SpentSumToNextLevelUp = spentSum };

            var ready = factory.ReadyForNextLevel;

            Assert.AreEqual(expected, ready);
        }
    }
}