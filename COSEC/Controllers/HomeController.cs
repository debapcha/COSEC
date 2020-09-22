using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using COSEC.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using COSEC.Repositories;

namespace COSEC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            //apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }

        [HttpGet]
        public IActionResult Index()
        {
            Login user = new Login();
            return View(user);
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

        // Method to authenticate user login   
        [HttpPost]
        public ActionResult Index(Login user)
        {
            DBAccess db = new DBAccess();
            if (db.CheckLogin(user))
            {
                if (user.Username.Equals("Approver1"))
                    return RedirectToAction("Index", "Users");
                if (user.Username.Equals("Approver2"))
                    return RedirectToAction("Index", "ApprovedUsers");
            }

            ModelState.Clear();
            ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
            return View();
        }
    }
}
