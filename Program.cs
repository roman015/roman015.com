using Blazored.LocalStorage;
using HomePage.Authorization;
using HomePage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HomePage
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration
                    .Bind("AzureAd", options.ProviderOptions.Authentication);

                options.ProviderOptions.DefaultAccessTokenScopes
                    .Add("api://f744ceb4-e6cf-4923-a971-c7fc4a600fe2/ApiScope");

                options.ProviderOptions.LoginMode = "redirect";
                
            });
            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy("BlogAdministratorsOnly", policy => policy.AddRequirements(new PermittedRoleRequirement("BlogAdministrator")));
            });

            #region For FE RBAC
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermittedRolePolicyProvider>();
            builder.Services.AddSingleton<IAuthorizationHandler, PermittedRoleAuthorizationHandler>();
            #endregion

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();

            builder.Services.AddScoped<BlogEditorAPIService>();

            await builder.Build().RunAsync();
        }
    }
}
