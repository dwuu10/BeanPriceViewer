using System.Diagnostics;
using BeanPriceViewer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeanPriceViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CityDbContext _context;

        public HomeController(ILogger<HomeController> logger, CityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CityView()
        {
            var allCities = _context.CitySet.ToList();
            return View(allCities);
        }

        public IActionResult CreateEditCity(int? id)
        {
            if(id != null)
            {
                // if not null, editing existing entity
                var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == id);
                return View(cityInDb);
            }

            return View();
        }

        public IActionResult Delete(int id)
        {
            var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == id);
            _context.CitySet.Remove(cityInDb);
            _context.SaveChanges();
            return RedirectToAction("CityView");
        }

        public IActionResult CreateEditCityForm(City model)
        {
            if(model.Id == 0)
            {
                // creating entity
                _context.CitySet.Add(model);
            }
            else
            {
                // editing entity
                _context.CitySet.Update(model);
            }

                _context.SaveChanges();
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
