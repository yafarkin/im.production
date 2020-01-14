using System;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryAddMaterialToStockChange : FactoryChange
    {
        public MaterialOnStock MaterialOnStock { get; protected set; }

        public FactoryAddMaterialToStockChange(GameTime time, Factory factory, MaterialOnStock materialOnStock, string description = null)
            : base(time, factory, null, null, null, description)
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
        }
    }
}