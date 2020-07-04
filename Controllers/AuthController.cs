using CookieAuthN.Models;
using CookieAuthN.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuthN.Controllers
{
    public class AuthController : Controller
    {
        private IUserService _userService;

        public AuthController(IUserService service)
        {
            _userService = service;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        public async Task<IActionResult> Logout()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("FormNotFilledOut", "Complete the form including all required fields");

                return View(model);
            }

            var isValid = await _userService.ValidateUserCredentialsAsync(model.UserName, model.Password);

            //Check if user is valid
            if (!isValid)
            {
                ModelState.AddModelError("InvalidCredentials", "Please check user name or password");
                return View(model);
            }
            var user = await _userService.GetUser(model.UserName);

            //Login User
            await LoginAsync(user);
            //Redirect user

            if (string.IsNullOrWhiteSpace(model.ReturnUrl) ||
                !Uri.IsWellFormedUriString(model.ReturnUrl, UriKind.Relative))
            {
                return View("Index", "Home");
            }
            //The user is logged in :D
            return Redirect(model.ReturnUrl);
        }

        private async Task LoginAsync(User model)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.DateOfBirth, model.DateOfBirth.ToLongDateString()),
                new Claim(ClaimTypes.Surname, model.LastName ?? ""),
                new Claim(ClaimTypes.GivenName, "Naruto"),
                new Claim(ClaimTypes.Actor, "Plain"),
                new Claim(ClaimTypes.Role,"dancer")

            };

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(2),
                AllowRefresh = false
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, authProperties);
        }
    }
}
