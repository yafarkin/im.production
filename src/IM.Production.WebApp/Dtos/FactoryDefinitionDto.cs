namespace IM.Production.WebApp.Dtos
{
    public class FactoryDefinitionDto
    {
        public int Level { get; set; }
        public int WorkersCount { get; set; }
        public decimal Cost { get; set; }
        public ProductionType ProductionType { get; set; }
    }
}
