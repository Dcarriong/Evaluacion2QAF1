using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using F1Drivers.Models;
using System;

namespace F1DriversApp.Controllers
{
    public class DriverController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string API_URL = "https://f1api.dev/api/drivers";

        public DriverController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetStringAsync(API_URL);
            var driverData = JsonConvert.DeserializeObject<ApiResponse>(response);

            if (driverData.Drivers == null || !driverData.Drivers.Any())
            {
                Console.WriteLine("No se encontraron conductores.");
            }

            var drivers = driverData.Drivers;
            var paginatedDrivers = drivers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)drivers.Count / pageSize);

            return View("Index", paginatedDrivers);
        }

        public async Task<IActionResult> Details(string id)
        {
            var response = await _httpClient.GetStringAsync($"{API_URL}/{id}");

            var driver = JsonConvert.DeserializeObject<Driver>(response);  

            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);  
        }
    }

    public class ApiResponse
    {
        public List<Driver> Drivers { get; set; }
    }
}
