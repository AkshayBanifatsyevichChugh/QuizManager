using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizManager.Application.Services;
using QuizManager.Domain.Entities;
using QuizManager.Infrastructure;
using System;

namespace QuizManager.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, IUserService userService)
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{action=Index}/{id?}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<QuizContext>()
                .AddIdentity<User, IdentityRole<long>>(o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequiredLength = 8;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireUppercase = true;
                    o.Lockout.AllowedForNewUsers = false;
                    o.Lockout.MaxFailedAccessAttempts = 999999;
                    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(1);
                })
                .AddEntityFrameworkStores<QuizContext>();
            services
                .AddControllersWithViews();
            services
                .AddRazorPages()
                .AddRazorRuntimeCompilation();

            RegisterDependencies(services);
        }

        private void RegisterDependencies(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IQuizService), typeof(QuizService));
            serviceCollection.AddScoped(typeof(IUserService), typeof(UserService));
            serviceCollection.AddSingleton(typeof(IMappingService), typeof(MappingService));
        }
    }
}