using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<Context>(opt => opt.UseInMemoryDatabase());
            services.AddMvc();
            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            var context = app.ApplicationServices.GetService<Context>();
            AddTestData(context);
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
                    name: "default",
                    template: "{controller=Notiz}/{action=Liste}/{id?}");
            });
        }

        private void AddTestData(Context context)
        {
            var notiz1 = new NotizDbModel
            {
                Erstelldatum = DateTime.Today,
                Beschreibung = "Mit diesem Programm kann man Notizen schreiben.",
                Wichtigkeit = 1,
                Titel = "Erste Notiz",
                Abgeschlossen = false,
                ErledigtBis = DateTime.Today.AddDays(1)
            };
            var notiz2 = new NotizDbModel
            {
                Erstelldatum = DateTime.Today,
                Beschreibung = "Am besten sollte man sich alles notieren.",
                Wichtigkeit = 1,
                Titel = "Nicht vergessen",
                Abgeschlossen = false,
                ErledigtBis = DateTime.Today.AddDays(3)
            };
            context.Add(notiz1);
            context.Add(notiz2);
            context.SaveChanges();
        }
    }
}
