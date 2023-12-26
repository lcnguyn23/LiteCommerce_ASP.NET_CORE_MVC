using Microsoft.AspNetCore.Mvc;
using SV20T1080003.Web.Models;
using System.Diagnostics;

namespace SV20T1080003.Web.Controllers
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
            var data = new HomeIndexModel()
            {
                TitleMessage = "List of Persons and Students",
                ListOfPersons = new PersonDAL().List(),
                ListOfStudents = new StudentDAL().List()
            };

            return View(data);
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

        public IActionResult InputEmployee()
        {
            return View();
        }

        public IActionResult GetEmployee(InputEmployee data)
        {
            
            return Content($"name: {data.Name}, age: {data.Age}");
        }
    }
}