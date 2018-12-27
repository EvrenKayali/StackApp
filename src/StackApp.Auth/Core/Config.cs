using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackApp.Auth
{
    public class Config
    {
        public static IEnumerable<Client> Clients = new List<Client>
        {
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireConsent = false,

                RedirectUris           = { "https://localhost:44319/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:44319/signout-callback-oidc" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Address,
                },
            },
    };

        public static IEnumerable<IdentityResource> IdentityResources = new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
        };

        public static IEnumerable<ApiResource> Apis = new List<ApiResource>
        {
            new ApiResource("api1", "My API 1")
        };

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
                {
                    UserClaims = { "role" }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mobile",

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = { "xamarinformsclients://callback" },
                    RequireConsent = false,
                    RequirePkce = true,
                    //PostLogoutRedirectUris = { $"{clientsUrl["Xamarin"]}/Account/Redirecting" },
                    //AllowedCorsOrigins = { "http://eshopxamarin" },

                    // scopes that client has access to
                    AllowedScopes = { "api1",
                                       IdentityServerConstants.StandardScopes.OpenId,
                                       IdentityServerConstants.StandardScopes.Profile,
                                       IdentityServerConstants.StandardScopes.OfflineAccess,
                    },

                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 30,
                },
                new Client
                {
                    ClientId = "adminUI",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = {
                        "https://az-app-moneybox-adminui.azurewebsites.net",
                        "https://az-app-moneybox-adminui.azurewebsites.net/unauthorized",
                        "https://az-app-moneybox-adminui.azurewebsites.net/silent-renew.html",
                        "http://localhost:4200",
                        "http://localhost:4200/unauthorized",
                        "http://localhost:4200/silent-renew.html",

                    },
                    AllowRememberConsent = true,
                    AllowedCorsOrigins = { "https://az-app-moneybox-adminui.azurewebsites.net",  "http://localhost:4200" },
                    PostLogoutRedirectUris = {
                        "http://localhost:4200/unauthorized",
                        "http://localhost:4200",
                        "https://az-app-moneybox-adminui.azurewebsites.net",
                        "https://az-app-moneybox-adminui.azurewebsites.net/unauthorized" },
                    // scopes that client has access to
                    AllowedScopes = { "api1",
                                       IdentityServerConstants.StandardScopes.OpenId,
                                       IdentityServerConstants.StandardScopes.Profile,
                                       IdentityServerConstants.StandardScopes.Email,
                                       "roles",
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 600,
                },
            };
        }

        public static List<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("roles", "Roles", new List<string>(){ "role" }),
                new IdentityResource("Full Name", "Full Name", new List<string>{"FullName"}),
            };
        }
    }
}
