using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace BeanPriceViewer;

    public class OpenWeatherMapAPI
    {
    public static int Weather(string cityname)
    {
        var client = new HttpClient();

        var city = cityname;
        var key = "c8751ce973594af2bed7f5ad6b9c57f5";

        var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=imperial&appid={key}";

        try
        {
            var response = client.GetStringAsync(weatherURL).Result;

            var formattedResponse = JObject.Parse(response).GetValue("main").ToString();

            var temp = JObject.Parse(formattedResponse).GetValue("temp");

            return (int)temp;
        } 
        catch (AggregateException e)
        {
            return -999;
        }
    }

        public static int Humidity(string cityname)
        {
            var client = new HttpClient();

            var city = cityname;
            var key = "c8751ce973594af2bed7f5ad6b9c57f5";

            var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=imperial&appid={key}";

            try
            {
                var response = client.GetStringAsync(weatherURL).Result;

                var formattedResponse = JObject.Parse(response).GetValue("main").ToString();

                var humid = JObject.Parse(formattedResponse).GetValue("humidity");

                return (int)humid;
            } 
            catch (AggregateException e)
            {
                return -999;
            }
        }

    }

