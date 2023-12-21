using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using University_System.Models;

namespace University_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}


        [Route("Home/Error")]
        public IActionResult Error(int? statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return View("Error400");
                case 404:
                    return View("Error404");
                case 500:
                    return View("Error500");
                case 503:
                    return View("Error502");
            }

            return View("Error");
        }
    }
}
