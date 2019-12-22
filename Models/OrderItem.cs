namespace ctl.Models
{
    public class OrderItem
    {
        public string Sku {get;set;}
        public int Quantity {get;set;} 
        public string Description {get;set;}
        public string UnitOfMeasure {get;set;}
        public decimal? DeclaredValuePerUOM {get;set;}
        public OrderVariable[] Variables {get;set;}
    }
}