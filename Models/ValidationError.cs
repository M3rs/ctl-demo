using Newtonsoft.Json.Linq;

namespace ctl.Models
{
    public class ValidationError
    {
        public string Message {get;set;}
        public JObject ModelState {get;set;}
    }
}