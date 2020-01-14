using System;
using System.Linq;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryRemoveEmptyMaterialsFromStockChange : FactoryChange
    {
        public FactoryRemoveEmptyMaterialsFromStockChange(GameTime time, Factory factory, string description = null)
            : base(time, factory, null, null, null, description)
        {
        }

        public override void DoAction()
        {
            var materialsToRemove = Factory.Stock.Where(m => m.Amount == 0).ToList();
            foreach (var m in materialsToRemove)
            {
                Factory._stock.Remove(m);
            }
        }
    }
}