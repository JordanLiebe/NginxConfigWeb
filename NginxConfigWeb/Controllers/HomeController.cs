﻿using System;
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

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            var Apps = await FirebaseInteractions.GetApplicationsAsync();

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
                    Output = ServerInteractions.StartServer();
                    break;
                case "Stop":
                    Output = ServerInteractions.StopServer();
                    break;
                case "Generate":
                    await ServerInteractions.UpdateConfig();
                    break;
                default:
                    Output = "Unknown Command";
                    break;
            }

            return RedirectToAction("Index", new { message = Output });
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
