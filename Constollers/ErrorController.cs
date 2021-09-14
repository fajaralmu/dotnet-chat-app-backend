using System;
using ChatAPI.Dto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Constollers
{
    [ApiController]
    public class ErrorController : BaseController
    {
        [Route("/error")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return 
            Json(WebResponse<string>.ErrorResponse(context.Error));
            // Problem(
            //     type: context.Error.GetType().Name,
            //     detail: context.Error.StackTrace,
            //     title: context.Error.Message);
        }

        // [Route("/error")]
        // public IActionResult Error() => Json(WebResponse<string>.ErrorResponse(context.Error));
    }
}