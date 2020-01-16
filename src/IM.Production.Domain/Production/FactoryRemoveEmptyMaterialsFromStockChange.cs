using System;
using System.Linq;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryRemoveEmptyMaterialsFromStockChange : FactoryChange
    {
        public FactoryRemoveEmptyMaterialsFromStockChange(Factory factory, string description = null)
            : base(factory, description)
        {
        }

        public override void DoAction()
        {
            var materialsToRemove = Factory._stock.Where(m => m.Amount == 0).ToList();
            foreach (var m in materialsToRemove)
            {
                Factory._stock.Remove(m);
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                Description = $"Удаление материалов со склада: {string.Join(',', materialsToRemove.Select(m => m.Material.DisplayName))}";
            }
        }
    }
}