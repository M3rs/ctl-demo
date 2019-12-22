using System;

namespace ctl.Models
{
    public class SalesOrder
    {
        public string CarrierId {get;set;}
        public DateTime DateOrdered {get;set;}
        public Address ShipTo {get;set;}
        public OrderItem[] Items {get;set;}
    }
}