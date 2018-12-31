using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackApp.Auth.IdentityModels;

namespace StackApp.Auth.Features.Membership
{
    public class MembershipController : Controller
    {
        private readonly AuthDbContext _db;
        public MembershipController(AuthDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
           var vm = await _db.Users.Select(u => new MemberListViewModel
                          {
                              FullName = u.FullName,
                              UserName = u.UserName,
                              BirthDate = u.BirthDate.ToShortDateString(),
                              Email = u.Email,
                              Id = u.Id.ToString(),
                          }).ToArrayAsync();

            return View(vm);
        }
    }
}