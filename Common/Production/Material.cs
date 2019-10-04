using Epam.ImitationGames.Production.Common.Base;
using System.Collections.Generic;

namespace Epam.ImitationGames.Production.Common.Production
{
    /// <summary>
    /// Материал.
    /// </summary>
    public class Material : VisibleEntity
    {
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

        public Material()
        {
            InputMaterials = new List<MaterialOnStock>();
        }
    }
}