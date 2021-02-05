using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository repository;

        public AppController(IMailService mailService, IDutchRepository repository)
        {
            this.mailService = mailService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                mailService.SendMessage(
                    to: "agembarzewska@gmail.com", 
                    subject: contactViewModel.Subject, 
                    body: $"From: {contactViewModel.Name} - {contactViewModel.Email}, Message: {contactViewModel.Message}");
                ViewBag.UserMessage = "Mail sent";
                ModelState.Clear();
            }

            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About us";
            return View();
        }

        [HttpGet("shop")]
        public IActionResult Shop()
        {
            var results = repository.GetAllProducts();
            return View(results);
        }
    }
}