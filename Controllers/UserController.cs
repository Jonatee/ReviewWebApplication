using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Review_Web_App.Constants;
using Review_Web_App.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;
using System.Security.Claims;
using System.Reflection.Metadata.Ecma335;

namespace Review_Web_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IReviewerService _reviewerService;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _cache;

        public UserController(IUserService userService, IReviewerService reviewerService, IMemoryCache cache, IEmailSender emailSender)
        {
            _reviewerService = reviewerService;
            _userService = userService;
            _cache = cache;
            _emailSender = emailSender;
        }

        public  IActionResult Registeration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registeration(ReviewerRequestModel model)
        {
           
             
            _cache.TryGetValue(model.Email, out string? storedCache);
            if(model.VerificationCode == storedCache)
            {
                var response = await _reviewerService.Create(model);
                if (response.Success)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Message = response.Message;
                    return View();
                }
            }
            else
            {
                return View();
            }
           

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendVerificationCode(string Email)
        {
            if (Email != null)
            {
                var code = new Random().Next(100000, 999999).ToString();

                _cache.Set(Email, code, TimeSpan.FromMinutes(10));

                var emailContent = $"Welcome to the Review Web App. Your Verification Code is {code}";
                var emailRequest = new EmailDto
                {

                    ToEmail = Email,
                    ToName = "User",
                    Subject = "Review App Verification Code",
                    HtmlContent = emailContent,
                };
                 var result =  _emailSender.SendEmail(emailRequest);
                if (result)
                {
                    return Json(new { success = true, message = "Verification code sent successfully. Press Back To Continue entering your Info" });
                }
                return Json(new { success = false, message = "Failed to Send Verification Code." });
            }
            

            return Json(new { success = false,message = "Email Not Found" });
        }

        public IActionResult CustomAccessDenied()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginRequest request)
        {
            var login = await _userService.Login(request);
            if (!login.Success)
            {
                TempData["ErrorMessage"] = login.Message;
                return View(request);
            }
            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, login.Data.Id.ToString()),
                new Claim(ClaimTypes.Name,login.Data.UserName),
                new Claim(ClaimTypes.Email, login.Data.Email),
                new Claim(ClaimTypes.Role, login.Data.Role.Name),

            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            
            var role = User.FindFirstValue(ClaimTypes.Role);
            if(role == RoleConstants.Admin)
            {
                return RedirectToAction("Index", "Category");
            }
            return RedirectToAction("Feed", "Posts");
        }

        public async Task<IActionResult> Logout()
        {
           await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           return RedirectToAction("Login");
        }
    }
}
