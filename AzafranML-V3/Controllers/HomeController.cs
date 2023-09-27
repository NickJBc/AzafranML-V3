using AzafranML_V3.Models;
using AzafranML_V3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AzafranML_V3.Controllers
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
        public IActionResult Predict()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Predict(PredictionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation failures
                return View(model);
            }

            // Your prediction code here

            // Assuming you want to display the results on the same page
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}