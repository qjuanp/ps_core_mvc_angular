using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Contact() => View();

        public IActionResult About() => View();
    }
}