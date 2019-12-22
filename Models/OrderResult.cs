namespace ctl.Models
{
    public class OrderResult : OrderData
    {
        public string Result {get;set;}
        public OrderResult(){}
        public OrderResult(OrderData data, string result)
        {
            OrderId = data.OrderId;
            FirstName = data.FirstName;
            LastName = data.LastName;
            Addr1 = data.LastName;
            City = data.City;
            State = data.State;
            Postal = data.Postal;
            SKU = data.SKU;
            Quantity = data.Quantity;
            Result = result;
        }
    }
}