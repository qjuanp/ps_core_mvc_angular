using System;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly DutchContext _conext;

        public AppController(
            IMailService mailService,
            DutchContext conext)
        {
            _mailService = mailService;
            _conext = conext;
        }
        public IActionResult Index() => View();

        [HttpGet("contact")]
        public IActionResult Contact() => View();

        [HttpPost("contact")]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _mailService.Send("test@gmail.com", $"From:{model.Name} Subject:{model.Subject}", $"Email:{model.Email} Message:{model.Message}");
                ViewBag.UserMessage = "Mail sent!";
                ModelState.Clear();
            }

            return View();
        }

        [HttpGet("about")]
        public IActionResult About() => View();

        public IActionResult Shop() => 
            View(_conext
                .Products
                .OrderBy(p => p.Category)
                .ToList());
    }
}