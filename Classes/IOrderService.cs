using ctl.Models;

namespace ctl.Classes 
{
    public interface IOrderService
    {
        OrderResult CreateOrder(OrderData order);
    }
}