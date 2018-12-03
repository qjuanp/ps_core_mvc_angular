using System;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet("contact")]
        public IActionResult Contact() => View();

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            return View();
        }

        [HttpGet("about")]
        public IActionResult About() => View();
    }
}