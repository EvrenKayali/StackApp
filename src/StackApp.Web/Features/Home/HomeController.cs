namespace StackApp.Web.Features.Home
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Reflection;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                HostName = Environment.MachineName,
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString()
            };
            return View(vm);
        }
    }
}