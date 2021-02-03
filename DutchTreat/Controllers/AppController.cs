using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;

        public AppController(IMailService mailService)
        {
            this.mailService = mailService;
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

        public IActionResult About()
        {
            ViewBag.Title = "About us";
            return View();
        }
    }
}