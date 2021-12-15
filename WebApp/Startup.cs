using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SOProject.DomainModels2;
using SOProject.ServiceLayer;
using System;
using System.Web.Http;
using System.Web.Optimization;
using Unity;
using Unity.Lifetime;

namespace WebApp
{
    public class Startup
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<IQuestionService, QuestionService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICategoryService, CategoryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAnswerService, AnswerService>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }

        public static void RegisterApiRoutes(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews(); 
            services.AddDbContext<SOProjectDbContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("SOPDB")));
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
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

            app.UseAuthentication();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            
        }
    }
}
