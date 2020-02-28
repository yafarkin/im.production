﻿using System;
using System.Collections.Generic;
using System.Linq;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Services;

namespace IM.Production.Services
{
    public class FactoriesService : IFactoriesService
    {
        private readonly Game _game;

        public IEnumerable<Factory> GetContractFactoriesByLogin(string login)
        {
            var factories = _game.Customers
                .Where(obj => obj.Login.Equals(login))
                .FirstOrDefault()?.Factories;
            return factories;
        }

        public IEnumerable<Factory> GetFactoriesByLogin(string login)
        {
            return _game.Customers.Where(obj => obj.Login.Equals(login)).FirstOrDefault()?.Factories;
        }
    }
}