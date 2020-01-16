using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryWorkerCountChange : FactoryChange
    {
        /// <summary>
        /// Изменение количества рабочих.
        /// </summary>
        public int NewWorkersCount { get; protected set; }

        public FactoryWorkerCountChange(Factory factory, int newWorkersCount, string description = null)
            : base(factory, description)
        {
            NewWorkersCount = newWorkersCount;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory.Workers = NewWorkersCount;
        }
    }
}