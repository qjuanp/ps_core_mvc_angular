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
        private readonly IDutchRepository _repository;

        public AppController(
            IMailService mailService,
            IDutchRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }

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

        public async Task<IActionResult> Shop() =>
            View(await _repository.GetAll());
    }
}