using System;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Base;

namespace Epam.ImitationGames.Production.Domain.Production
{
    [Serializable]
    public class FactoryProductionMaterialChange : FactoryChange
    {
        public IList<Material> Materials { get; protected set; }

        public FactoryProductionMaterialChange(GameTime time, Factory factory, IList<Material> materials, string description = null)
            : base(time, factory, null, null, null, description)
        {
            Materials = materials;
        }

        public override void DoAction()
        {
            base.DoAction();
            Factory.ProductionMaterials = Materials;
        }
    }
}