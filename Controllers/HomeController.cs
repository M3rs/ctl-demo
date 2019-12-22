using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ctl.Models;
using ctl.Classes;
using System.IO;

namespace ctl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;
        private readonly IOrderService OrderService;

        public HomeController(ILogger<HomeController> logger, IOrderService orderService)
        {
            Logger = logger;
            OrderService = orderService;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Clear();

            return View();
        }

        [HttpPost]
        public IActionResult UploadOrder(IFormFile file)
        {
            HttpContext.Session.Set("filename", file.FileName);
            HttpContext.Session.Set("results", new List<OrderResult>());

            using (var sr = new StreamReader(file.OpenReadStream())) {
                var data = Ctl.Data.Formats.Csv.ReadObjects<OrderData>(sr);
                return Json(new { file.FileName, data = data.ToArray() });
            }
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderData data)
        {
            var result = OrderService.CreateOrder(data);
            var results = HttpContext.Session.Get<List<OrderResult>>("results");
            results.Add(result);
            HttpContext.Session.Set("results", results);

            return Json(result);
        }

        public IActionResult Results()
        {
            var filename = HttpContext.Session.Get<string>("filename");
            var vm = new ResultsViewModel {
                Filename = filename,
            };

            return View(vm);
        }

        public IActionResult GetResultData()
        {
           var results = HttpContext.Session.Get<List<OrderResult>>("results"); 
           return Json(results);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
