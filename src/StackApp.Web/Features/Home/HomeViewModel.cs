using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackApp.Web.Features.Home
{
    public class HomeViewModel
    {
        public string HostName { get; set; }
        public string Version { get; set; }
        public string Authority { get; set; }
        public string Name { get; set; }
        public string Roles { get; set; }
    }
}
