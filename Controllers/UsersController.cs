using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Constollers;
using ChatAPI.Dto;
using ChatAPI.Helper;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{

    [Authorize]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet, Route("api/user/{id}")]
        public ActionResult<WebResponse<User>> Read(int id)
        {
            return CommonJson(_userService.Read(id));
        }

        [HttpGet, Route("api/user/profile")]
        public ActionResult<WebResponse<User>> Profile()
        {
            User user = (User)HttpContext.Items["User"];
            return CommonJson(user);
        }
        [HttpPut, Route("api/user/profile")]
        public ActionResult<WebResponse<User>> UpdateProfile(UserProfileDto profileDto)
        {
            return CommonJson(_userService.UpdateProfile(profileDto, GetLoggedUser()));
        }

    }
}
