using System;  
using System.Threading.Tasks;
using ChatAPI.Dto;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ChatAPI.Middlewares
{
    public class CustomFilterMiddleware
    {
        private readonly RequestDelegate next;

        public CustomFilterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            Console.WriteLine("PATH: "+context.Request.Path);
            await next(context);
            
        }

        // private Task HandleExceptionAsync(HttpContext context, Exception exception)
        // {
        //     context.Response.ContentType = "application/json";

        //     WebResponse<string> result = new WebResponse<string>()
        //     {
        //         Success = 0,
        //         Message = exception.GetType().Name,
        //         Result = exception.Message
        //     };
        //     return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        // }
    }
}