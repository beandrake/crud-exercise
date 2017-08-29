using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PassengerManager.Data;
using Microsoft.EntityFrameworkCore;

namespace PassengerManager
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

            // For local development, the ASP.NET Core configuration system reads the connection string from the appsettings.json file.
            services.AddDbContext<OverallContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            /*
             * While it may be helpful at some later stage of development,
             * I didn't see the need for the Home Page at present.
             * I wanted to user to immediately see Passengers/Index.cshtml,
             * which has the interface needed to view and edit Passengers.
             * I found the solution here: https://stackoverflow.com/questions/1142003/set-homepage-in-asp-net-mvc           
             */

            // this sets the default content that appears in place of
            // @RenderBody in the file: Shared/_Layouts.cshtml
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Passengers}/{action=Index}/{id?}");
            });
        }
    }
}
