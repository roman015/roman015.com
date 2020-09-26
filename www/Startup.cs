using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureB2C;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using static AzureB2C.AzureAdB2CAuthenticationBuilderExtensions;
using MySQL.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Discord.WebSocket;

namespace www.roman015.com
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Civ 6 Turn Notification Handler
            services.AddSingleton<DiscordSocketClient>();
            services.AddSingleton<Civ6TurnNotificationHandler>();            
            #endregion

            #region Add Repository/DBContext
            services.AddDbContext<UrlShortenerContext>(options =>
                    options.UseMySQL(Configuration["UrlShortenerConnectionString"])
                    );
            services.AddTransient(typeof(UrlShortenerRepository));
            #endregion

            #region Add Authentication
            services.Configure<AzureAdB2COptions>(Configuration.GetSection("Authentication:AzureAdB2C"));
            services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, OpenIdConnectOptionsSetup>();

            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAdB2C(options => Configuration.Bind("Authentication:AzureAdB2C", options))
            .AddCookie();
            #endregion

            #region Add Pages
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorPagesOptions(options => {
                    options.Conventions.AuthorizeFolder("/Factorio");
                }); 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //IdentityModelEventSource.ShowPII = true;                
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
    }
}
