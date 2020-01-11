using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Описание изменения, произошедшего с фабрикой
    /// </summary>
    [Serializable]
    public abstract class FactoryChange : BaseChanging
    {
        /// <summary>
        /// Ссылка на фабрику.
        /// </summary>
        public Factory Factory { get; protected set; }

        /// <summary>
        /// Изменение уровня.
        /// </summary>
        public int? NewLevel { get; protected set; }

        /// <summary>
        /// Изменение количества рабочих.
        /// </summary>
        public int? NewWorkersCount { get; protected set; }

        /// <summary>
        /// Изменение % исследования по RD.
        /// </summary>
        public decimal? NewRDProgress { get; set; }

        /// <summary>
        /// Изменение суммы, выделяемой на RD.
        /// </summary>
        public decimal? NewSumOnRD { get; protected set; }

        protected FactoryChange(GameTime time, Factory factory, int? newLevel, int? newWorkersCount, decimal? newSumOnRD, string description = null)
            : base(time, factory.Customer, description)
        {
            Factory = factory;
            NewLevel = newLevel;
            NewWorkersCount = newWorkersCount;
            NewSumOnRD = newSumOnRD;
        }

        public override void DoAction()
        {
            if (NewLevel.HasValue)
            {
                Factory.Level = NewLevel.Value;
            }

            if (NewWorkersCount.HasValue)
            {
                Factory.Workers = NewWorkersCount.Value;
            }

            if(NewSumOnRD.HasValue)
            {
                Factory.SumOnRD = NewSumOnRD.Value;
            }
        }
    }
}