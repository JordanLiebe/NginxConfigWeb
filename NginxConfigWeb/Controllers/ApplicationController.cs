using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NginxConfigWeb.Models;
using NginxConfigWeb.Tools;

namespace NginxConfigWeb.Controllers
{
    public class ApplicationController : Controller
    {
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

            await FirebaseInteractions.CreateApplication(CreatedApp);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Modify(string Id)
        {
            var app = await FirebaseInteractions.GetApplicationByIdAsync(Id);

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

            await FirebaseInteractions.UpdateApplication(UpdatedApp);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var app = await FirebaseInteractions.GetApplicationByIdAsync(Id);

            await FirebaseInteractions.RemoveApplication(app);

            return RedirectToAction("Index", "Home");
        }
    }
}
