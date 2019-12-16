using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;

namespace CyberSecurityBase.Feedback.Idp
{    
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryApiResources(apiResources)
                .AddInMemoryClients(clients)
                .AddTestUsers(TestUsers.Users);
        }
        
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();
            app.UseIdentityServer();            
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var role = new IdentityResource(
                name: "role",
                displayName: "role",
                claimTypes: new[] { "role"});

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                role
            };
        }

        private readonly List<ApiResource> apiResources = new List<ApiResource>
        {
            new ApiResource("feedback-api")
        };
        
        private readonly List<Client> clients = new List<Client>
        {
            new Client
            {
                ClientId = "CyberSecurityBase.Feedback.Web",
                ClientName = "Feedback App",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = new List<string> {"openid", "profile", "role", "email", "feedback-api"},
                RedirectUris = new List<string> { "https://localhost:44376/auth-callback", "https://localhost:44376/silent-refresh.html"},
                PostLogoutRedirectUris = new List<string> {"https://localhost:44376/"},
                AllowedCorsOrigins = new List<string> {"https://localhost:44376"},
                AllowAccessTokensViaBrowser = true
            }
        };
    }
}