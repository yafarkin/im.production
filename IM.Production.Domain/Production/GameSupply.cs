using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Список изначальных ресурсов, поставляемых игрой.
    /// </summary>
    public class GameSupply : BaseEntity
    {
        public List<MaterialWithPrice> Materials;
    }
}