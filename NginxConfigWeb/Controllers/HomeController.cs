using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NginxConfigWeb.Models;
using NginxConfigWeb.Tools;

namespace NginxConfigWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _fireBaseToken;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _fireBaseToken = configuration.GetValue<string>("FireBaseKey");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var Apps = await FirebaseInteractions.GetApplicationsAsync(_fireBaseToken);

            if (message != "" && message != null)
                ViewBag.message = message;
            else
                ViewBag.message = "System Good";

            return View(Apps);
        }

        [HttpPost]
        public async Task<IActionResult> Control(string Action)
        {
            string Output = "";

            switch(Action)
            {
                case "Start":

                    _logger.LogInformation("Starting Server...");
                    Output = ServerInteractions.StartServer();
                    _logger.LogInformation(Output);
                    break;
                case "Stop":
                    _logger.LogInformation("Stopping Server...");
                    Output = ServerInteractions.StopServer();
                    _logger.LogInformation(Output);
                    break;
                case "Generate":
                    _logger.LogInformation("Generating Config...");
                    Output = await ServerInteractions.UpdateConfig(_logger);
                    break;
                default:
                    _logger.LogInformation("Unknown Command");
                    break;
            }

            return RedirectToAction("Index", new { message = Output });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help(string Page)
        {
            return View("Help", Page);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
