using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolWatyAnalytics.Web.Data;

[assembly: HostingStartup(typeof(SolWatyAnalytics.Web.Areas.Identity.IdentityHostingStartup))]
namespace SolWatyAnalytics.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SolWatyAnalyticsWebContext>(options =>
                    options.UseNpgsql(
                        context.Configuration.GetConnectionString("localPgSQL")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<SolWatyAnalyticsWebContext>();
            });
        }
    }
}