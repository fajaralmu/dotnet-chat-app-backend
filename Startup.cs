using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChatAPI.Context;
using ChatAPI.Middlewares;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            Console.WriteLine("===============Conn String: " + connectionString);
            services.AddMvc(options =>
                        options.EnableEndpointRouting = false
                    )
                    .AddJsonOptions(options =>
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    )
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //DB Context
            services.AddDbContext<ChatAppContext>
                    (optionsAction => optionsAction.UseNpgsql(connectionString));

            //Services
            services.AddScoped<UserService, UserService>();
            services.AddScoped<SettingService, SettingService>();
            services.AddScoped<ChatService, ChatService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };
            webSocketOptions.AllowedOrigins.Add("http://localhost:4200");
            app.UseWebSockets(webSocketOptions); //WebSocketMiddleware
            app.UseMiddleware<CustomFilterMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            // if (env.IsDevelopment())
            // {
            //     app.UseExceptionHandler("/error-local-development");
            // }
            // else
            // {
            app.UseExceptionHandler("/error");
            // }

            app.UseMvc();
        }
    }

}
