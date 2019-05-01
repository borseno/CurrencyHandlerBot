using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CurrencyHandler.Models;

namespace CurrencyHandler.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "hahahaha";
        }

        public string About()
        {
            ViewData["Message"] = "Your application description page.";

            return "about";
        }

        public string Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return "contact";
        }

        public string Privacy()
        {
            return "privacy";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
