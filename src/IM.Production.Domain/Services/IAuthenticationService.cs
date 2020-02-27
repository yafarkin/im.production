namespace Epam.ImitationGames.Production.Domain.Services
{
    public interface IAuthenticationService
    {
        string Authenticate(string login, string password);
    }
}
