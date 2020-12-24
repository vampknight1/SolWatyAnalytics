using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SolWatyAnalytics.Api.Models;
using SolWatyAnalytics.Api.Repo;
using SolWatyAnalytics.Web.Data;
using SolWatyAnalytics.Web.Hubs;
using SolWatyAnalytics.Web.Services;

namespace SolWatyAnalytics.Web
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });

            //services.AddDefaultIdentity<IdentityUser>();
                //.AddRoles<IdentityRole>()
                //.AddEntityFrameworkStores<SolWatyAnalyticsWebContext>();
            services.AddAuthentication("Identity.Application")
                .AddCookie();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true;
            }).AddBootstrapProviders().AddFontAwesomeIcons();

            services.AddHttpClient<IStationXService, StationXService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44338");
            });
            services.AddHttpClient<ICustomerService, CustomerService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44338");
            });
            services.AddHttpClient<IAreaService, AreaService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44338");
            });
            services.AddHttpClient<IStationService, StationService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44338");
            });
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<HttpClient>();


            //services.AddDbContext<AppDbContext>(
            //    options => options.UseNpgsql(
            //        _config.GetConnectionString("localPgSQL")));
            //services.AddScoped<StationXRepo>();
            //services.AddSingleton<StationXService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();            

            app.ApplicationServices
                .UseBootstrapProviders()
                .UseFontAwesomeIcons();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<BroadcastHub>("/broadcastHub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}