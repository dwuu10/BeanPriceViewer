using System.Diagnostics;
using BeanPriceViewer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeanPriceViewer.Controllers
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

        public IActionResult CityView()
        {
            return View();
        }

        public IActionResult CreateEditCity()
        {
            return View();
        }

        public IActionResult CreateEditCityForm(City model)
        {

            return RedirectToAction("CityView");
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
