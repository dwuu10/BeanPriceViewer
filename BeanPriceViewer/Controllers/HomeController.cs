using System.Diagnostics;
using BeanPriceViewer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeanPriceViewer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly CityDbContext _context;

        private readonly GameDbContext _contextgame;

        public HomeController(ILogger<HomeController> logger, CityDbContext context, GameDbContext contextgame)
        {
            _logger = logger;
            _context = context;
            _contextgame = contextgame;
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

        public IActionResult CreateCity()
        {
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
            model.Temperature = OpenWeatherMapAPI.Weather(model.Name);

            if (model.Temperature == -999)
            {
                return RedirectToAction("SeeCityError");
            }

            model.Humidity = OpenWeatherMapAPI.Humidity(model.Name);
            model.BluePrice = CityData.CalculateBlueBeanPrice((int)model.Temperature);
            model.RedPrice = CityData.CalculateRedBeanPrice((int)model.Temperature);
            model.GreenPrice = CityData.CalculateGreenBeanPrice((int)model.Humidity);
            model.YellowPrice = CityData.CalculateYellowBeanPrice((int)model.Humidity);

            if (model.Id == 0)
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

        // game stuff

        public IActionResult GamesView()
        {
            var allGames = _contextgame.Games.ToList();
            ViewBag.Games = allGames;
            return View();
        }

        public IActionResult NewGame()
        {
            return View();
        }

        public IActionResult NewGameForm(GameData model)
        {
            if (model.MaxTurns <= 0 || model.MaxTurns == null)
            {
                model.MaxTurns = 20;
            }

            if (model.Cash <= 0 || model.Cash == null)
            {
                model.Cash = 1000;
            }

            model.Turn = 0;
            model.BlueStock = 0;
            model.YellowStock = 0;
            model.GreenStock = 0;
            model.RedStock = 0;

            if (model.Id == 0)
            {
                // creating entity
                _contextgame.Games.Add(model);
            }
            else
            {

                // editing entity
                _contextgame.Games.Update(model);
            }

            _context.SaveChanges();
            return RedirectToAction("GameMain", model);
        }

        public IActionResult GameMain(GameData model)
        {
            var allCities = _context.CitySet.ToList();
            ViewBag.Cities = allCities;
            ViewBag.Game = model;

            return View();
        }

        public IActionResult NextTurn(GameData model)
        {
            var gameInDb = _contextgame.Games.SingleOrDefault(x => x.Id == model.Id);
            gameInDb.Turn += 1;
            _contextgame.Games.Update(gameInDb);
            _contextgame.SaveChanges();
            return RedirectToAction("GameMain", gameInDb);
        }

        public IActionResult BuyMenu(GameData model, int cid)
        {
            if (cid != null)
            {
                ViewBag.GameData = model;
                // if not null, editing existing entity
                var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == cid);
                return View(cityInDb);
            }
            return View();
        }

        public IActionResult SellMenu(GameData model, int cid)
        {
            if (cid != null)
            {
                ViewBag.GameData = model;
                // if not null, editing existing entity
                var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == cid);
                return View(cityInDb);
            }
            return View();
        }

        public IActionResult Purchase(GameData model, int cid, int amount, string type)
        {
            var allCities = _context.CitySet.ToList();
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            if (type == "Blue")
            {
                price = (int) city.BluePrice; 
            }
            else if (type == "Red")
            {
                price = (int) city.RedPrice;
            }
            else if (type == "Green")
            {
                price = (int) city.GreenPrice;
            }
            else
            {
                price = (int) city.YellowPrice;
            }
            var cost = price * amount;

            if (cost > model.Cash)
            {
                return RedirectToAction("InvalidTransaction", model);
            } 
            ViewBag.Cost = cost;
            ViewBag.Model = model;

            return View();
        }

        public IActionResult ConfirmPurchase(GameData model, int cid, int amount, string type)
        {
            model.Turn++;
            var allCities = _context.CitySet.ToList();
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            if (type == "Blue")
            {
                price = (int)city.BluePrice;
                model.BlueStock += amount;
            }
            else if (type == "Red")
            {
                price = (int)city.RedPrice;
                model.RedStock += amount;
            }
            else if (type == "Green")
            {
                price = (int)city.GreenPrice;
                model.GreenStock += amount;
            }
            else
            {
                price = (int)city.YellowPrice;
                model.YellowStock += amount;
            }
            var cost = price * amount;

            if (cost > model.Cash)
            {
                return RedirectToAction("InvalidTransaction", model);
            }
            else
            {
                model.Cash -= cost;
            }

            _contextgame.Games.Update(model);
            _contextgame.SaveChanges();
            return RedirectToAction("GameMain", model);
        }

        public IActionResult Sell(GameData model, int cid, int amount, string type)
        {
            var allCities = _context.CitySet.ToList();
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            var stock = 0;
            if (type == "Blue")
            {
                price = (int)city.BluePrice;
                stock = (int)model.BlueStock;
            }
            else if (type == "Red")
            {
                price = (int)city.RedPrice;
                stock = (int)model.RedStock;
            }
            else if (type == "Green")
            {
                price = (int)city.GreenPrice;
                stock = (int)model.GreenStock;
            }
            else
            {
                price = (int)city.YellowPrice;
                stock = (int)model.YellowStock;
            }

            var value = price * amount;

            if (amount > stock)
            {
                return RedirectToAction("InvalidTransaction", model);
            }
            ViewBag.Cost = amount;
            ViewBag.Model = model;

            return View();
        }

        public IActionResult ConfirmSale(GameData model, int cid, int amount, string type)
        {
            model.Turn++;
            var allCities = _context.CitySet.ToList();
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            if (type == "Blue")
            {
                price = (int)city.BluePrice;
                model.BlueStock -= amount;
            }
            else if (type == "Red")
            {
                price = (int)city.RedPrice;
                model.RedStock -= amount;
            }
            else if (type == "Green")
            {
                price = (int)city.GreenPrice;
                model.GreenStock -= amount;
            }
            else
            {
                price = (int)city.YellowPrice;
                model.YellowStock -= amount;
            }
            var cost = price * amount;

            model.Cash += cost;

            _contextgame.Games.Update(model);
            _contextgame.SaveChanges();
            return RedirectToAction("GameMain", model);
        }

        public IActionResult DeleteGame(int id)
        {
            var gameInDb = _contextgame.Games.SingleOrDefault(x => x.Id == id);
            _contextgame.Games.Remove(gameInDb);
            _contextgame.SaveChanges();
            return RedirectToAction("GamesView");
        }

        public IActionResult GameOver(GameData model)
        {
            return View(model);
        }

        public IActionResult InvalidTransaction(GameData model)
        {
            ViewBag.Model = model;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
