using System;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryAddMaterialToStockChange : FactoryChange
    {
        public MaterialOnStock MaterialOnStock { get; protected set; }

        public FactoryAddMaterialToStockChange(Factory factory, MaterialOnStock materialOnStock, string description = null)
            : base(factory, description)
        {
            MaterialOnStock = materialOnStock;
        }

        public override void DoAction()
        {
            base.DoAction();
            if (MaterialOnStock != null)
            {
                ReferenceData.ReferenceData.AddMaterialToStock(Factory._stock, MaterialOnStock);
            }

            if (string.IsNullOrEmpty(Description))
            {
                Description = $"Внесение {MaterialOnStock} на склад";
            }
        }
    }
}