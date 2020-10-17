using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace Dashboard.Controllers
{
   
    public class HomeController : Controller
    {
        public HomeController()
        {
        }


       
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                return RedirectPermanent("/");
            } else
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectPermanent("/Login");
            }  
        }

        internal static object Index()
        {
            throw new NotImplementedException();
        }
    }
}