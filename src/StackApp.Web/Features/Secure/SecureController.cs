using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StackApp.Web.Features.Secure
{
    public class SecureController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}