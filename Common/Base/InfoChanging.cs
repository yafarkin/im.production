namespace Epam.ImitationGames.Production.Common.Base
{
    /// <summary>
    /// Класс для информирования игрока о чём либо
    /// </summary>
    public class InfoChanging : BaseChanging
    {
        public InfoChanging(GameTime time, Customer customer, string description)
            : base(time, customer, description)
        {
        }
    }
}