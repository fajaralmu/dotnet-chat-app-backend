using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ChatAPI.Models;
using ChatAPI.Services;
using ChatAPI.Context;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;
using ChatAPI.Middlewares;

namespace ChatAPI
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration["Data:ChatAPIConnection:ConnectionStrings:DefaultConnection"];
            Console.WriteLine("===============Conn String: "+connectionString);
            services.AddMvc(options => 
                        options.EnableEndpointRouting = false
                    )
                    .AddJsonOptions(options => 
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    )
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            //DB Context
            services.AddDbContext<UserContext>
                    (optionsAction=>optionsAction.UseNpgsql(connectionString));

            //Services
            services.AddScoped<UserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CustomFilterMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

             app.UseMvc(); 
        }
    }
 
}
