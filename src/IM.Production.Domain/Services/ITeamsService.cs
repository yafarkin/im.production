﻿using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface ITeamsService
    {
        IEnumerable<Customer> GetTeams();
        bool AddTeam(Customer team);
    }
}
