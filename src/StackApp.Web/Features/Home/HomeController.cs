namespace StackApp.Web.Features.Home
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;

    public class HomeController : Controller
    {

        public HomeController()
        {

        }
        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                HostName = Environment.MachineName,
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Authority = Environment.GetEnvironmentVariable("Authority")
            };

            if (User.Identity.IsAuthenticated)
            {
                vm.Name = User.Claims.First(c => c.Type == "name").Value;
            }
            return View(vm);
        }
    }
}