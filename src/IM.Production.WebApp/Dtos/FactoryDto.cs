using System;

namespace IM.Production.WebApp.Dtos
{
    public class FactoryDto
    {
        public Guid Id { get; set; }
        public string ProductionTypeKey { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Performance { get; set; }
        public int Workers { get; set; }
        public int SumOnRD { get; set; }
        public int NeedSumToNextLevelUp { get; set; }
        public int SpentSumToNextLevelUp { get; set; }
        public int RdProgress { get; set; }
        public int TotalOnRD { get; set; }
        public int TotalOnTaxes { get; set; }
        public int TotalOnSalary { get; set; }
        public int TotalExpenses { get; set; }
    }
}
