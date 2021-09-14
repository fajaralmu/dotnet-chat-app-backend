using System;
using ChatAPI.Dto;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChatAPI.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(WebResponse<string>.ErrorResponse(
                    new UnauthorizedAccessException("Unauthorized")
                )) { 
                    StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}