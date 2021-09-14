
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
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly UserService _userService;
        public AuthController(UserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost, Route("login")]
        public ActionResult<WebResponse<User>> Login([FromForm] IFormCollection value)
        {
            User result = _userService.Login(value);
            Response.Headers.Add("access-token", result.Token);
            return CommonJson(result);
        }

        [Authorize]
        [HttpGet, Route("profile")]
        public ActionResult<WebResponse<User>> Profile()
        {
            User user = (User) HttpContext.Items["User"];
            return CommonJson(user);
        }
    }
}