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

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var ConfigFileLocation = _configuration.GetSection("configuration").GetSection("configFileLocationWindows").Value;
            ConfigReadWrite.ReadConfig(ConfigFileLocation);

            List<string> apps = new List<string>();

            apps.Add("Jmliebe");
            apps.Add("Bethlehem");

            return View(apps);
        }

        public IActionResult Update()
        {
            return View();
        }

        public IActionResult submit(string name, string live, string record, string[] push_urls)
        {
            RtmpApplications rtmpApp = new RtmpApplications()
            {
                name = name,
                live = live,
                record = record,
                push_urls = push_urls
            };

            return RedirectToAction("Index");
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
