using Bus.DAL.Context;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Globalization;
using WebBusServer.Services;
using WebBusServer.Services.Base;

namespace WebBusServer
{


    public class Startup
    {

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {



         

            services.AddDbContext<BusDB>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            });

           // services.AddTransient<ISeedDataService, SeedDataBuses>();
            services.AddTransient<ISeedDataService, SeedDataTrips>();

            services.AddControllers();
            services.AddHttpClient();
            services.AddSession(options =>
            {
                // Налаштування опцій сесій
                options.Cookie.Name = ".AspNetCore.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDistributedMemoryCache();
            services.AddDistributedMemoryCache();
           


        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           
            
           


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


          
          /* using (var scope = app.ApplicationServices.CreateScope())
            {
                var seedDataBuses = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
                seedDataBuses.Initialize();
            }*/

          using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var seedDataTrips = serviceScope.ServiceProvider.GetRequiredService<ISeedDataService>();
                seedDataTrips.Initialize();
            }
          
        }
    }
}
