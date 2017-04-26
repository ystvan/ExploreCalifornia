using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExploreCalifornia
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IHostingEnvironment env)
        {
            configuration = new ConfigurationBuilder()
                                    .AddEnvironmentVariables()
                                    .AddJsonFile(env.ContentRootPath + "/config.json")

                                    //the second parameter boolean set to true tells that this configuration file is OPTIONAL!
                                    //This parameter tells the configuration API that if the file exists at run time, then yeah, 
                                    //sure, go ahead and read it in. 
                                    //Otherwise if the file is missing, just keep going with the configuration that you've already got.
                                    .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                                    .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            /*
             * Dependency Injection options:
             *  TRANSIENT: 
                 *  AddTransient method will have a transient or the shortest lifespan, 
                 *  as ASP.NET Core will create a new instance every time one is requested. 
             *  SCOPED: 
                 *  the AddScoped method, it generally means that ASP.NET Core will only 
                 *  create one instance of that type for each web request. Sharing state between different
                 *  components components throughout the same request without worrying about another user's
                 *  request gaining access to that same data.
             *  SINGLETON:
                 * the AddSingleton method, this method will only create one instance of each type for 
                 * the entire lifetime of the application, which is helpful in cases when you have some 
                 * common data that you want to share across all users or when you have a type that's 
                 * particularly expensive to create and is not specific to any particular user or request. 
                 
             */
            services.AddTransient<FeatureToggles>(x => new FeatureToggles
            {
                EnableDeveloperExceptions = configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions")
            });

            //registering service
            services.AddMvcCore();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            FeatureToggles features
            )
        {
            loggerFactory.AddConsole();

            app.UseExceptionHandler("/error.html");

            

            //if (configuration["EnableDeveloperExceptions"] == "True")
            
            if(features.EnableDeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }

            /*
            //2 input parameter NEXT middleware function
            app.Use(async (context, next) =>
            {
                //if evalueates false, will skip and go to *
                if (context.Request.Path.Value.StartsWith("/hello"))
                {
                    await context.Response.WriteAsync("Hello from Cali!");
                }
                // * the next()
                await next();
            });
            //so far this is the only one single piece of middleware registered, and that is the app.Run method
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
            */

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("invalid"))
                    throw new Exception("ERROR");
                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id}");
            });

            //rendering any static files content it can be found under the 'wwwroot' folder
            app.UseFileServer();
            
        }
    }
}
