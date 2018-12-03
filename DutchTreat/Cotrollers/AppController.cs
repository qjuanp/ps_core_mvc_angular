using System;
using System.Threading.Tasks;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;

        public AppController(IMailService mailService) => _mailService = mailService;

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
    }
}