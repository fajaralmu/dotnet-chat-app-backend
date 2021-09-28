using System;
using System.Text.Json.Serialization;
using ChatAPI.Context;
using ChatAPI.Middlewares;
using ChatAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //DB Context
            services.AddDbContext<ChatAppContext>
                    (optionsAction => optionsAction.UseNpgsql(connectionString));

            services.AddSignalR();

            //Services
            services.AddSingleton<WebsocketService, WebsocketService>();
            services.AddScoped<UserService, UserService>();
            services.AddScoped<SettingService, SettingService>();
            services.AddScoped<ChatService, ChatService>();
            services.AddSwaggerGen();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rover Simulation V1 API");
            });

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
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapHub<ChatHub>("/chatHub");
            });
            

            app.UseMvc();
        }
    }

}
