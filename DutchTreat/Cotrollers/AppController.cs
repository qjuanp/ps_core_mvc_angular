using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet("contact")]
        public IActionResult Contact() => View();

        [HttpGet("about")]
        public IActionResult About() => View();
    }
}