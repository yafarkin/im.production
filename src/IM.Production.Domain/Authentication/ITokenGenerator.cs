using System.Collections.Generic;
using System.Security.Claims;

namespace Epam.ImitationGames.Production.Domain.Authentication
{
    public interface ITokenGenerator
    {
        string Generate(IEnumerable<Claim> claims);
    }
}
