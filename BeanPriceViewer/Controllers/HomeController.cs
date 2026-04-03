using BeanPriceViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

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
            return View(allGames);
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

            _contextgame.SaveChanges();
            return RedirectToAction("GamesView");
        }

        public IActionResult PlayGame(int? id)
        {
            if (id != null)
            {
                var gameInDb = _contextgame.Games.SingleOrDefault(x => x.Id == id);
                return RedirectToAction("GameMain", gameInDb);
            }

            return RedirectToAction("GameNotFound");
        }

        public IActionResult GameMain(GameData model)
        {
            if (model.Turn > model.MaxTurns)
            {
                return RedirectToAction("GameOver", model);
            }
            var allCities = _context.CitySet.ToList();
            ViewBag.Cities = allCities;
            ViewBag.Cash = model.Cash;
            ViewBag.Turn = model.Turn;
            ViewBag.MaxTurn = model.MaxTurns;

            return View(model);
        }

        public IActionResult NextTurn(GameData model)
        {
            model.Turn += 1;
            _contextgame.Games.Update(model);
            _contextgame.SaveChanges();
            return RedirectToAction("GameMain", model);
        }

        // buy/sell menus

        public IActionResult BuyMenu(int cid, int gameid)
        {
            var gameInDb = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);
            var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == cid);


            if (gameInDb != null && cityInDb != null)
            {
                ViewBag.CID = cid;
                ViewBag.GameId = gameid;
                ViewBag.Cash = gameInDb.Cash;
                ViewBag.Turns = gameInDb.Turn;
                ViewBag.MaxTurns = gameInDb.MaxTurns;
                ViewBag.BluePrice = cityInDb.BluePrice;
                ViewBag.RedPrice = cityInDb.RedPrice;
                ViewBag.YellowPrice = cityInDb.YellowPrice;
                ViewBag.GreenPrice = cityInDb.GreenPrice;
                ViewBag.CityName = cityInDb.Name;

                return View();
            }
            else if (cityInDb == null)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }
            else
            {
                return RedirectToAction("GameNotFound");
            }
                
        }

        public IActionResult SellMenu(int cid, int gameid)
        {
            var gameInDb = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);
            var cityInDb = _context.CitySet.SingleOrDefault(x => x.Id == cid);

            if (gameInDb != null && cityInDb != null)
            {
                ViewBag.CID = cid;
                ViewBag.GameId = gameid;
                ViewBag.Cash = gameInDb.Cash;
                ViewBag.Turns = gameInDb.Turn;
                ViewBag.MaxTurns = gameInDb.MaxTurns;
                ViewBag.BluePrice = cityInDb.BluePrice;
                ViewBag.RedPrice = cityInDb.RedPrice;
                ViewBag.YellowPrice = cityInDb.YellowPrice;
                ViewBag.GreenPrice = cityInDb.GreenPrice;
                ViewBag.CityName = cityInDb.Name;

                return View();
            }
            else if (cityInDb == null)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }
            else
            {
                return RedirectToAction("GameNotFound");
            }
        }

        // bean purchasing

        public IActionResult Purchase(TransactionData input, int cid, int gameid)
        {
            if (input.Amount < 1)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }

            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var model = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);

            var price = 0;
            var beanType = input.Type;
            string beanString = "";
            if (beanType == 1)
            {
                price = (int)city.BluePrice;
                beanString = "Blue";
            }
            else if (beanType == 2)
            {
                price = (int)city.RedPrice;
                beanString = "Red";
            }
            else if (beanType == 3)
            {
                price = (int)city.GreenPrice;
                beanString = "Green";
            }
            else
            {
                price = (int)city.YellowPrice;
                beanString = "Yellow";
            }
            var cost = price * input.Amount;

            if (cost > model.Cash)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }
            ViewBag.Cost = cost;
            ViewBag.Model = model;
            ViewBag.CID = cid;
            ViewBag.GameId = gameid;
            ViewBag.Amount = input.Amount;
            ViewBag.Type = input.Type;
            ViewBag.BeanString = beanString;

            return View();

        }

        public IActionResult ConfirmPurchase(int cid, int gameid, int amount, int type)
        {
            var model = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);

            var price = 0;
            if (type == 1)
            {
                price = (int)city.BluePrice;
                model.BlueStock += amount;
            }
            else if (type == 2)
            {
                price = (int)city.RedPrice;
                model.RedStock += amount;
            }
            else if (type == 3)
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
                return RedirectToAction("TransactionError", new { id = gameid });
            }
            else
            {
                model.Cash -= cost;
            }

            model.Turn++;

            _contextgame.Games.Update(model);
            _contextgame.SaveChanges();
            return RedirectToAction("GameMain", model);

        }

        // bean selling

        public IActionResult Sell(TransactionData input, int cid, int gameid)
        {
            if (input.Amount < 1)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }

            var model = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            var stock = 0;
            var beanType = input.Type;
            string beanString = "";
            if (beanType == 1)
            {
                price = (int)city.BluePrice;
                stock = (int)model.BlueStock;
                beanString = "Blue";
            }
            else if (beanType == 2)
            {
                price = (int)city.RedPrice;
                stock = (int)model.RedStock;
                beanString = "Red";
            }
            else if (beanType == 3)
            {
                price = (int)city.GreenPrice;
                stock = (int)model.GreenStock;
                beanString = "Green";
            }
            else
            {
                price = (int)city.YellowPrice;
                stock = (int)model.YellowStock;
                beanString = "Yellow";
            }

            var value = price * input.Amount;

            if (input.Amount > stock)
            {
                return RedirectToAction("TransactionError", new { id = gameid });
            }

            ViewBag.Cost = value;
            ViewBag.Model = model;
            ViewBag.CID = cid;
            ViewBag.GameId = gameid;
            ViewBag.Amount = input.Amount;
            ViewBag.Type = input.Type;
            ViewBag.BeanString = beanString;


            return View(model);
        }

        public IActionResult ConfirmSale(int cid, int gameid, int amount, int type)
        {
            var model = _contextgame.Games.SingleOrDefault(x => x.Id == gameid);
            var city = _context.CitySet.SingleOrDefault(x => x.Id == cid);
            var price = 0;
            var stock = 0;
            if (type == 1)
            {
                price = (int)city.BluePrice;
                stock = (int)city.BluePrice;
                model.BlueStock -= amount;
            }
            else if (type == 2)
            {
                price = (int)city.RedPrice;
                stock = (int)city.RedPrice;
                model.RedStock -= amount;
            }
            else if (type == 3)
            {
                price = (int)city.GreenPrice;
                stock = (int)city.GreenPrice;
                model.GreenStock -= amount;
            }
            else
            {
                price = (int)city.YellowPrice;
                stock = (int)city.YellowPrice;
                model.YellowStock -= amount;
            }

            if (amount > stock)
            {
                return RedirectToAction("TransactionError", new{ id = gameid });
            }

            var cost = price * amount;

            model.Cash += cost;
            model.Turn++;

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
            ViewBag.Cash = model.Cash;
            ViewBag.MaxTurns = model.MaxTurns;
            ViewBag.Blue = model.BlueStock;
            ViewBag.Red = model.RedStock;
            ViewBag.Yellow = model.YellowStock;
            ViewBag.Green = model.GreenStock;
            return View();
        }

        // error handling

        public IActionResult TransactionError(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GameNotFound");
            }

            ViewBag.Id = id;

            return View();
        }

        public IActionResult GameNotFound()
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
