
using System.Net;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatAPI.Dto;

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
        
        [HttpPost]
        [Route("login")]
        public ActionResult<WebResponse<User>> Login([FromForm] IFormCollection value)
        {
            return CommonJson(_userService.Login(value));
        }
    }
}