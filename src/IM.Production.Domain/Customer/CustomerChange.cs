using Epam.ImitationGames.Production.Domain.Bank;
using Epam.ImitationGames.Production.Domain.Base;
using Epam.ImitationGames.Production.Domain.Production;

namespace Epam.ImitationGames.Production.Domain
{
    /// <summary>
    /// Изменения в команде.
    /// </summary>
    public class CustomerChange : BaseChanging
    {
        /// <summary>
        /// Заключенный контракт.
        /// </summary>
        public Contract NewContract { get; set; }

        /// <summary>
        /// Закрытый контракт.
        /// </summary>
        public Contract ClosedContract { get; set; }

        /// <summary>
        /// Купленая фабрика.
        /// </summary>
        public Factory BoughtFactory { get; set; }

        /// <summary>
        /// Проданная фабрика.
        /// </summary>
        public Factory SoldFactory { get; set; }

        /// <summary>
        /// Команда, у которой фабрика куплена или продана (если фабрика покупается/продаётся игре - то не задаётся).
        /// </summary>
        public Customer OtherCustomer { get; set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal RDProgressChange { get; set; }

        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal SumOnRDChange { get; set; }

        /// <summary>
        /// Изменение уровня поколения фабрик.
        /// </summary>
        public int FactoryGenerationLevelChange { get; set; }

        /// <summary>
        /// Ссылка на банковскую операцию.
        /// </summary>
        public BankFinAction FinAction { get; set; }

        /// <summary>
        /// Изменения суммы на счёту команды.
        /// </summary>
        public decimal SumChange { get; set; }

        public CustomerChange(GameTime time, Customer customer, string description = null)
            : base(time, customer, description)
        {
        }
    }
}