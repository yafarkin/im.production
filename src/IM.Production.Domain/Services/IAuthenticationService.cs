using IM.Production.Services;

namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(string login, string password);
    }
}
