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
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseExceptionHandler("/error.html");

            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/config.json")

                //the second parameter boolean set to true tells that this configuration file is OPTIONAL!
                //This parameter tells the configuration API that if the file exists at run time, then yeah, 
                //sure, go ahead and read it in. 
                //Otherwise if the file is missing, just keep going with the configuration that you've already got.
                .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                .Build();

            //if (configuration["EnableDeveloperExceptions"] == "True")
            if (configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions"))
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

            //rendering any static files content it can be found under the 'wwwroot' folder
            app.UseFileServer();
            
        }
    }
}
