using IOption.Models;
using IOption.Models.ViewModels;
using IOption.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOption
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
            services.AddControllersWithViews();

            services.AddScoped<KavehNegarService>();
            services.AddScoped<ParsGreenNegarService>();
            services.AddScoped<Func<SelectSmsPanel, IsmsServices>>(ServiceProvider => result =>
            {
                switch (result)
                {
                    case SelectSmsPanel.KavehNegar:
                        return ServiceProvider.GetService<KavehNegarService>();
                    case SelectSmsPanel.ParsGreen:
                        return ServiceProvider.GetService<ParsGreenNegarService>();
                    default:
                        return ServiceProvider.GetService<KavehNegarService>();
                }
            });

            services.Configure<KavehNegarViewModel>(Configuration.GetSection("kavehNegarApi"));
            services.Configure<PasargadViewModel>(Configuration.GetSection("Pasargad"));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
