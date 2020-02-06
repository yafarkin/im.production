using System;
using Epam.ImitationGames.Production.Domain.Base;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Domain.Production
{
    /// <summary>
    /// Материал.
    /// </summary>
    [Serializable]
    public class Material : VisibleEntity
    {
        public Material()
        {
            InputMaterials = new List<MaterialOnStock>();
        }

        public string Key { get; set; }

        /// <summary>
        /// К какой области производства относится данный материал.
        /// </summary>
        public ProductionType ProductionType { get; set; }

        /// <summary>
        /// Требуемые входные материалы с их количеством.
        /// </summary>
        public List<MaterialOnStock> InputMaterials { get; set; }

        /// <summary>
        /// Скорость производства материала.
        /// </summary>
        public decimal AmountPerDay;
    }
}