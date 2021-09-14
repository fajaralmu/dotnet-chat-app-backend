using System;
using System.Linq.Expressions;
using System.Net;
using ChatAPI.Dto;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Constollers
{
    public class BaseController : Controller
    {

        protected ActionResult<WebResponse<T>> CommonJson<T>(T returnValue)
        {
            return Json(
                WebResponse<T>.SuccessResponse(returnValue)
            );
        }

        protected User GetLoggedUser()
        {
            try
            {
                return (User)HttpContext.Items["User"];
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}