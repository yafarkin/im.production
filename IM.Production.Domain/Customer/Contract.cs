using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    /// <summary>
    /// Описание заключенного контракта.
    /// </summary>
    public class Contract : MaterialLogistic
    {
        /// <summary>
        /// Штрафы за не поставку материала, за каждую единицу, если предусмотрено.
        /// </summary>
        public decimal Fine { get; set; }

        /// <summary>
        /// Сумма страховой премии (производителю), за не поставку материала, если предусмотрено.
        /// </summary>
        public decimal SrcInsurancePremium { get; set; }

        /// <summary>
        /// Страховая выплата (производителю), если материал не был поставлен, если предусмотрено.
        /// </summary>
        public decimal SrcInsuranceAmount { get; set; }

        /// <summary>
        /// Сумма страховой премии (получателю), за не поставку материала, если предусмотрено.
        /// </summary>
        public decimal DestInsurancePremium { get; set; }

        /// <summary>
        /// Страховая выплата (получателю), если материал не был поставлен, если предусмотрено.
        /// </summary>
        public decimal DestInsuranceAmount { get; set; }

        public Contract(GameTime time, MaterialWithPrice materialWithPrice, string description = null)
            : base(time, materialWithPrice, description)
        {
        }
    }
}