using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Notizen.DbModel;
using Notizen.DbModel.Notizen;

namespace Notizen
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase());
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var context = app.ApplicationServices.GetService<ApplicationDbContext>();
            AddTestData(context);

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("de-ch")),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("de-ch")
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("de-ch")
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Notiz/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Notiz}/{action=Liste}/{id?}");
            });
        }

        private void AddTestData(ApplicationDbContext context)
        {
            var notiz1 = new NotizDbModel
            {
                Erstelldatum = DateTime.Now.AddDays(-2).AddHours(2).AddMinutes(-12),
                Beschreibung = "Mit diesem Programm kann man Notizen schreiben.",
                Wichtigkeit = 1,
                Titel = "Erste Notiz",
                AbgeschlossenZeitpunkt = null,
                ErledigtBis = DateTime.Now.AddDays(1).AddHours(4).AddMinutes(16)
            };
            var notiz2 = new NotizDbModel
            {
                Erstelldatum = DateTime.Now.AddDays(-1).AddHours(-1).AddMinutes(44),
                Beschreibung = "Am besten sollte man sich alles notieren.",
                Wichtigkeit = 5,
                Titel = "Nicht vergessen",
                AbgeschlossenZeitpunkt = DateTime.Now.AddHours(-1),
                ErledigtBis = DateTime.Now.AddDays(3).AddHours(7).AddMinutes(34)
            };
            context.Add(notiz1);
            context.Add(notiz2);
            context.SaveChanges();
        }
    }
}