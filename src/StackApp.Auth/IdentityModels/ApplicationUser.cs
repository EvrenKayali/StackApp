namespace StackApp.Auth.IdentityModels
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class ApplicationUser : IdentityUser<Guid>
    {
        public DateTime BirthDate { get; set; }
        public string FullName { get; set; }
    }
}
