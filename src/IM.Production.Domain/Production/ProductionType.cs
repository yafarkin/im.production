using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Тип производства.
    /// </summary>
    public class ProductionType : BaseProduction, IVisibleEntity
    {
        public string Key { get; set; }

        public string DisplayName { get; set; }

        public override string ToString() => DisplayName;
    }
}