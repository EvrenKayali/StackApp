using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace StackApp.Web.Features.Auth
{
    public class AuthController : Controller
    {
        public async Task Login(string returnUrl = null)
        {
            // clear any existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync("oidc");

            // see IdentityServer4 QuickStartUI AccountController ExternalLogin
            await HttpContext.ChallengeAsync("oidc",
                new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("Index", "Home"),
                });
        }

        public async Task Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

    }
}