
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PRN222_TaskManagement.Models;
using PRN222_TaskManagement.Services;
using System.Security.Claims;

namespace AssignmentPRN392.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Route("logout")]
        public  async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User account)
        {

            if (account.Email == null || account.PasswordHash == null)
            {
                return View();
            }
            var user = await _userService.GetByEmailAsync(account.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email does not exist");
                return View();
            }
            if (account.PasswordHash != user.PasswordHash )
            {
                ModelState.AddModelError("PasswordHash", "Password is incorrect");
                return View();
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role?.ToString()?? "user")

                };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }
        [Route("access-denied")]
        public IActionResult AccessDined()
        {
            return View();
        }
        
    }
}
