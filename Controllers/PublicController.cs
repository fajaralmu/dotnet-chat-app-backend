
using System.Net;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatAPI.Dto;
using System;
using ChatAPI.Helper;

namespace ChatAPI.Constollers
{
    [Route("api/public")]
    [ApiController]
    public class PublicController : BaseController
    {
        private readonly SettingService _settingService;
        public PublicController(SettingService settingService)
        {
            _settingService = settingService;
        }
        
        [HttpGet, Route("index")]
        public ActionResult<WebResponse<ApplicationProfile>> Index()
        {
            WebResponse<ApplicationProfile> response = new WebResponse<ApplicationProfile>();
            response.Result = _settingService.GetProfile();
            if (HttpContext.Items["User"] != null)
            {
                response.User = (User) HttpContext.Items["User"];
            }
            return Json(response);
        }
    }
}