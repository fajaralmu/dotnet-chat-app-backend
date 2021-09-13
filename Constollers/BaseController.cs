using System.Net;
using Microsoft.AspNetCore.Mvc;
using ChatAPI.Dto;

namespace ChatAPI.Constollers
{
    public class BaseController: Controller
    {
        
        protected ActionResult<WebResponse<T>> CommonJson<T>(T returnValue)
        {
            return Json(
                WebResponse<T>.SuccessResponse(returnValue)
            );
        }
    }
}