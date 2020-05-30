using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClubWestRFC.DataAccess;
using ClubWestRFC.DataAccess.Data.Repository;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using ClubWestRFC.Utility;

namespace ClubWestRFC
{
    public class Startup

        // una wS HERE
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Allow member to log into account even it has not been confirmed

            services.AddIdentity<IdentityUser, IdentityRole>()
                //for emails
                .AddDefaultTokenProviders()                 
                .AddEntityFrameworkStores<ApplicationDbContext>();

           // services.AddSingleton<IEmailSender, EmailSender>();




            //adding Iunitof and Unit of work to project so it can used by contollers
            services.AddScoped<IUnitofWork, UnitofWork>();




            //added MVC and disables endpoints
            services.AddMvc(options=>options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            //adding controller with views AddRazorRuntimeCompilation
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //Remove Razor pages Added RazorRunTimeCompliation
            //services.AddRazorPages().AddRazorRuntimeCompilation();


            /*
             Microsoft has changed few things with Identity vs DefaultIdentity 
             which we updated in startup.cs a while ago.
              Because of which the default paths to login/logout 
              and access denied fails.
             */

            services.ConfigureApplicationCookie(options =>

            {

                options.LoginPath = $"/Identity/Account/Login";

                options.LogoutPath = $"/Identity/Account/Logout";

                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            //NOT needed
            //app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            //Add MVC (both MVC and razor pages)
            app.UseMvc();

            /*
             * Remove endpoints code
             * 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
            */
        }
    }
}
