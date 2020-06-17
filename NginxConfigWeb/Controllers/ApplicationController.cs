using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NginxConfigWeb.Models;
using NginxConfigWeb.Tools;

namespace NginxConfigWeb.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _fireBaseToken;

        public ApplicationController(IConfiguration configuration)
        {
            _configuration = configuration;
            _fireBaseToken = configuration.GetValue<string>("FireBaseKey");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, string live, string record, string[] push_urls)
        {
            RtmpApplications CreatedApp = new RtmpApplications
            {
                name = name,
                live = live,
                record = record,
                push_urls = push_urls
            };

            await FirebaseInteractions.CreateApplication(CreatedApp, _fireBaseToken);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Modify(string Id)
        {
            var app = await FirebaseInteractions.GetApplicationByIdAsync(Id, _fireBaseToken);

            return View(app);
        }

        [HttpPost]
        public async Task<IActionResult> Modify(string name, string live, string record, string[] push_urls)
        {
            RtmpApplications UpdatedApp = new RtmpApplications
            {
                name = name,
                live = live,
                record = record,
                push_urls = push_urls
            };

            await FirebaseInteractions.UpdateApplication(UpdatedApp, _fireBaseToken);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var app = await FirebaseInteractions.GetApplicationByIdAsync(Id, _fireBaseToken);

            await FirebaseInteractions.RemoveApplication(app, _fireBaseToken);

            return RedirectToAction("Index", "Home");
        }
    }
}
