using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings;
using Newtonsoft.Json;
using ctl.Models;

namespace ctl.Classes 
{
    public class ApiOrderService : IOrderService
    {
        readonly string API_URL = "http://api-test.ctlglobalsolutions.com";
        readonly HttpClient Client;

        public ApiOrderService(string userid, string clientid)
        {
            //UserId = userid;
            //ClientId = clientid;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-Ctl-ClientId", clientid);
            Client.DefaultRequestHeaders.Add("X-Ctl-UserId", userid);
        }

        public OrderResult CreateOrder(OrderData order)
        {
            var so = new SalesOrder {
                ShipTo = new Address {
                    Name = $"{order.FirstName} {order.LastName}",
                    Address1 = order.Addr1,
                    City = order.City,
                    StateOrProvince = order.State,
                    PostalCode = order.Postal,
                },
                Items = new OrderItem[] {
                    new OrderItem {
                        Sku = order.SKU,
                        Quantity = order.Quantity ?? 0,
                    }
                }
            };

            var resp = Client.PutAsJsonAsync($"{API_URL}/v1/Orders/{order.OrderId}", so).Result;

            return new OrderResult(order, GetResultMessage(resp));
        }

        private string GetResultMessage(HttpResponseMessage resp)
        {
           switch (resp.StatusCode) {
                case HttpStatusCode.Created: return "Created!";
                case HttpStatusCode.Unauthorized: return "Unauthorized";
                case HttpStatusCode.Conflict: return "An order with this ID already exists and may not be updated";
                case HttpStatusCode.ServiceUnavailable: return "There is a temporary error preventing the request from completing.";
                case HttpStatusCode.BadRequest:
                    var ve = JsonConvert.DeserializeObject<ValidationError>(resp.Content.ReadAsStringAsync().Result);
                    var sb = new StringBuilder();
                    sb.Append(ve.Message);
                    foreach (var kvp in ve.ModelState) {
                        sb.Append($"{kvp.Key}: {kvp.Value}");
                    }
                    return sb.ToString();
                default:
                    return "Unknown error in order creation";
            }
        }
    }
}