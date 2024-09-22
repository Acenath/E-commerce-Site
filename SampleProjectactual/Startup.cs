using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace SampleProjectactual
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Corrected: Use _configuration instead of Configuration
            services.Configure<StripeSettings>(_configuration.GetSection("Stripe"));

            services.AddControllersWithViews(); // Adding MVC Architectural pattern

            // Configure DbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.Cookie.Path = "/";
                });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("In Production environment");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            logger.LogInformation("Enabling HTTPS Redirection");
            app.UseHttpsRedirection();

            logger.LogInformation("Enabling Static Files");
            app.UseStaticFiles();

            logger.LogInformation("Routing Middleware Enabled");
            app.UseRouting();

            logger.LogInformation("Enabling Authentication Middleware");
            app.UseAuthentication();

            logger.LogInformation("Enabling Authorization Middleware");
            app.UseAuthorization();

            logger.LogInformation("Enabling Session Middleware");
            app.UseSession();

            logger.LogInformation("Configuring Endpoints");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            logger.LogInformation("Application Pipeline Configuration Complete");
        }
    }
}
