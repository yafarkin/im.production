using System;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryProductionMaterialChange : FactoryChange
    {
        public IList<Material> Materials { get; protected set; }

        public FactoryProductionMaterialChange(Factory factory, IList<Material> materials, string description = null)
            : base(factory, description)
        {
            Materials = materials;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory._productionMaterials = new List<Material>(Materials);
        }
    }
}