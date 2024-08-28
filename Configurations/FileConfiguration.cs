using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Mvc;

namespace Review_Web_App.Configurations
{
    public class FileConfiguration
    {
        public string Path { get; set; }
    }


    public class CustomAuthorizationFilter : Microsoft.AspNetCore.Mvc.Filters.IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
            else if (!context.HttpContext.User.IsInRole("Admin"))
            {
                context.Result = new RedirectToActionResult("CustomAccessDenied", "User", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }

}