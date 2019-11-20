using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    public class TaxOnFactory : BaseEntity
    {
        /// <summary>
        /// Ссылка на определение фабрики.
        /// </summary>
        public FactoryDefinition FactoryDefinition { get; set; }

        /// <summary>
        /// % налога (0...1).
        /// </summary>
        public decimal Percent { get; set; }
    }
}