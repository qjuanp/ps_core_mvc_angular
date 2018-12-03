using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IHostingEnvironment _env;

        public AppController(IHostingEnvironment env) => _env = env;
        public IActionResult Index()
        {
            return View();
        }
    }
}