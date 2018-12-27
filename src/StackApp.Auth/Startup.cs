using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackApp.Auth.Extensions;
using StackApp.Auth.IdentityModels;

namespace StackApp.Auth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = @"Server=.\sqlexpress;Database=StackApp.Auth;Integrated Security=true;MultipleActiveResultSets=true";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MembershipDb")));

            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(options =>
                {
                    options.ConfigureFeatureFolders();
                });

            services.AddIdentity<ApplicationUser, ApplicationRole>(Options => {
                Options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 0,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false,
                };
            })
               .AddEntityFrameworkStores<AuthDbContext>();

            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30; // interval in seconds
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>();

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.Apis)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }

        private void InitializeMembers(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
                context.Database.Migrate();

                if (!context.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                    
                    CreateUser(userManager, roleManager, "Admin", "Evren Kayali");
                    CreateUser(userManager, roleManager, "Cashier", "Sema Donmez");
                    CreateUser(userManager, roleManager, "Cashier", "Turgut Guney");
                    CreateUser(userManager, roleManager, "CompanyOwner", "Erkan Yanilmaz");
                    CreateUser(userManager, roleManager, "Client", "Ercan Akin");
                    CreateUser(userManager, roleManager, "Client", "Recep Sonmez");
                    CreateUser(userManager, roleManager, "Client", "Hasan Uncu");
                    CreateUser(userManager, roleManager, "Client", "Ertem Gonul");
                    CreateUser(userManager, roleManager, "Client", "Merve Uzgun");

                    context.SaveChanges();
                }


            }
        }

        static void CreateUser(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            string roleName,
            string fullName)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                ApplicationRole userRole = new ApplicationRole();
                userRole.Name = roleName;
                userRole.Description = "Perform admin operations.";
                IdentityResult roleResult = roleManager.CreateAsync(userRole).Result;
            }

            ApplicationUser user = new ApplicationUser();
            user.UserName = fullName.ToUserName();
            user.Email = $"{fullName.ToEmail()}@example.com";
            user.FullName = fullName;
            user.BirthDate = new DateTime().RandomDate();

            IdentityResult result = userManager.CreateAsync(user, roleName).Result;

             if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, roleName).Wait();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeDatabase(app);
            InitializeMembers(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
