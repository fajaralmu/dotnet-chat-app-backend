using System;
using System.Linq;
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
            Console.WriteLine($"{context.Request.Method} : {context.Request.Path}");

            PopulateResponseForCors(context.Response);
            
            if (!context.Request.Method.ToLower().Equals("options"))
            {
                await next(context);
            }

            Console.WriteLine("ResponseCode:" + context.Response.StatusCode);

        }

        private void PopulateResponseForCors(HttpResponse response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Expose-Headers", "access-token, requestid");
            response.Headers.Add("Access-Control-Allow-Credentials", "true");
            response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS, DELETE");
            response.Headers.Add("Access-Control-Max-Age", "3600");
            response.Headers.Add("Access-Control-Allow-Headers",
                    "Content-Type, Accept, X-Requested-With, Authorization, requestid, access-token");
            response.StatusCode = StatusCodes.Status200OK;
        }
    }
}