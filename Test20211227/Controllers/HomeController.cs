using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test20211227.Models;

namespace Test20211227.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(OpenWeatherMap openWeatherMap, string myCity)
        {
             
            openWeatherMap = FillCity();
            List<ResponseWeather> WeatherList = new List<ResponseWeather>();

            if (myCity == null) {
                myCity = "Jakarta";
            }
            string apiKey = "f33c591e2fadfc2ac5dd1508993ed78b";
            HttpWebRequest apiRequest = 
            WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?q="+ myCity + "&appid=f33c591e2fadfc2ac5dd1508993ed78b") as HttpWebRequest;
            string apiResponse = "";
            using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                apiResponse = reader.ReadToEnd();
            }
            ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table><tr><th>"+ myCity + " Weather</th></tr>");
            sb.Append("<tr><td>City:</td><td>" +
            rootObject.name + "</td></tr>");
            sb.Append("<tr><td>Country:</td><td>" +
            rootObject.sys.country + "</td></tr>");
            sb.Append("<tr><td>Wind:</td><td>" +
            rootObject.wind.speed + " Km/h</td></tr>");
            sb.Append("<tr><td>Current Temperature:</td><td>" +
            rootObject.main.temp + " °C</td></tr>");
            sb.Append("<tr><td>Humidity:</td><td>" +
            rootObject.main.humidity + "</td></tr>");
            sb.Append("<tr><td>Weather:</td><td>" +
            rootObject.weather[0].description + "</td></tr>");
            sb.Append("</table>");
            openWeatherMap.apiResponse = sb.ToString();

            return View(openWeatherMap);
        }
        public async Task<JsonResult> getCityCountries() {
            List<Country> CountryList = new List<Country>();
            apiResponse obj;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://countriesnow.space/api/v0.1/countries/iso"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                     obj = JsonConvert.DeserializeObject<apiResponse>(apiResponse);
                     
                }
            } 
            return Json(new { data = obj.data }, new System.Text.Json.JsonSerializerOptions());
        } 



        public OpenWeatherMap FillCity()
        {
            OpenWeatherMap openWeatherMap = new OpenWeatherMap();
            openWeatherMap.cities = new Dictionary<string, string>();
            openWeatherMap.cities.Add("Melbourne", "7839805");
            openWeatherMap.cities.Add("Auckland", "2193734");
            openWeatherMap.cities.Add("New Delhi", "1261481");
            openWeatherMap.cities.Add("Abu Dhabi", "292968");
            openWeatherMap.cities.Add("Lahore", "1172451");
            return openWeatherMap;
        }

        public async Task<JsonResult> getWeather(string city)
        {
            List<ResponseWeather> WeatherList = new List<ResponseWeather>();
           
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=jakarta&appid=f33c591e2fadfc2ac5dd1508993ed78b"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ResponseWeather rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                     WeatherList.Add(rootObject);
                }
            }

            return Json(new { data = WeatherList }, new Newtonsoft.Json.JsonSerializerSettings());
            // return View(reservationList);
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
