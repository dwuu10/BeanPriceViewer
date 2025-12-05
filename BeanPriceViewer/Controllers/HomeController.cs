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
                model.Temperature = OpenWeatherMapAPI.Weather(model.Name);
                model.Humidity = OpenWeatherMapAPI.Humidity(model.Name);
                model.BluePrice = CityData.CalculateBlueBeanPrice((int)model.Temperature);
                model.RedPrice = CityData.CalculateRedBeanPrice((int)model.Temperature);
                model.GreenPrice = CityData.CalculateGreenBeanPrice((int)model.Humidity);
                model.YellowPrice = CityData.CalculateYellowBeanPrice((int)model.Humidity);
                _context.CitySet.Update(model);
            }

                _context.SaveChanges();
            return RedirectToAction("CityView");
        }

        public IActionResult SeeCity(City model)
        {
            model.Temperature = OpenWeatherMapAPI.Weather(model.Name);
            model.Humidity = OpenWeatherMapAPI.Humidity(model.Name);
            if (model.Name == null || model.Temperature == -999)
            {
                return RedirectToAction("SeeCityError");
            }
            else
            {
                model.BluePrice = CityData.CalculateBlueBeanPrice((int)model.Temperature);
                model.RedPrice = CityData.CalculateRedBeanPrice((int)model.Temperature);
                model.GreenPrice = CityData.CalculateGreenBeanPrice((int)model.Humidity);
                model.YellowPrice = CityData.CalculateYellowBeanPrice((int)model.Humidity);
                return View(model);
            }
        }

        public IActionResult SeeCityError()
        {
            return View();
        }

        public IActionResult UpdateEntireDb()
        {
            var allCities = _context.CitySet.ToList();
            foreach (var city in allCities)
            {
                city.Temperature = OpenWeatherMapAPI.Weather(city.Name);
                city.Humidity = OpenWeatherMapAPI.Humidity(city.Name);
                city.BluePrice = CityData.CalculateBlueBeanPrice((int)city.Temperature);
                city.RedPrice = CityData.CalculateRedBeanPrice((int)city.Temperature);
                city.GreenPrice = CityData.CalculateGreenBeanPrice((int)city.Humidity);
                city.YellowPrice = CityData.CalculateYellowBeanPrice((int)city.Humidity);
                _context.CitySet.Update(city);
                _context.SaveChanges();
            }
            return RedirectToAction("CityView");
        }

        public IActionResult About()
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
