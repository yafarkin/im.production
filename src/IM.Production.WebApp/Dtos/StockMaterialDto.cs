namespace IM.Production.WebApp.Dtos
{
    public class StockMaterialDto
    {
        public string Key { get; set; }
        public string ProductionType { get; set; }
        public int Amount { get; set; }
        public int ProduceAmountPerDay { get; set; }
        public int SellAmountPerDay { get; set; }
    }
}