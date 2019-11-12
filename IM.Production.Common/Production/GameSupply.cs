using System.Collections.Generic;
using Epam.ImitationGames.Production.Common.Base;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Список изначальных ресурсов, поставляемых игрой.
    /// </summary>
    public class GameSupply : BaseEntity
    {
        public List<MaterialWithPrice> Materials;
    }
}