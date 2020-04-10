using Epam.ImitationGames.Production.Domain.Authentication;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(string login, string password);
    }
}