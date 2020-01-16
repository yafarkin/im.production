using System;

namespace Epam.ImitationGames.Production.Domain.Base
{
    /// <summary>
    /// Класс для информирования игрока о чём либо
    /// </summary>
    [Serializable]
    public class InfoChanging : BaseChanging
    {
        public InfoChanging(Customer customer, string description)
            : base(customer, description)
        {
        }
    }
}