using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface ITeamsService
    {
        IEnumerable<Customer> GetTeams();
        void AddTeam(Customer team);
        TeamProgress GetTeamProgress(string login);
    }
}