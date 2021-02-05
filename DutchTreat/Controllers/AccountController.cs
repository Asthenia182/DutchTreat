using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<StoreUser> signInManager;
        private readonly UserManager<StoreUser> userManager;
        private readonly IConfiguration config;

        public AccountController(ILogger<AccountController> logger,
            SignInManager<StoreUser> signInManager, 
            UserManager<StoreUser> userManager,
            IConfiguration config)
        {
            Logger = logger;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.config = config;
        }

        public ILogger<AccountController> Logger { get; }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                var result = signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (result.IsCompletedSuccessfully)
                {
                    //Create the token
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        config["Tokens:Issuer"],
                        config["Tokens:Audience"],
                        claims,
                        expires:DateTime.UtcNow.AddMinutes(30),
                        signingCredentials:creds
                        );

                    var results = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    };
                }
            }

            return BadRequest();
        }

        public IActionResult Login()
        {
            return this.User.Identity.IsAuthenticated ? RedirectToAction("Index", "App") : (IActionResult)View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName,
                    model.Password,
                    model.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    return Request.Query.Keys.Contains("ReturnUrl")
                        ? Redirect(Request.Query["ReturnUrl"].First())
                        : (IActionResult)RedirectToAction("Shop", "App");
                }
            }
            else
            {
                ModelState.AddModelError("", "Failed to login");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}