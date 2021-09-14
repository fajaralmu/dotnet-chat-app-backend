using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Constollers;
using ChatAPI.Dto;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<WebResponse<IEnumerable<User>>> Index()
        {
            return CommonJson(_userService.getUsers());
        }
        [HttpPost]
        [Route("register")]
        public ActionResult<WebResponse<User>> Register([FromForm]IFormCollection value)
        {
            return CommonJson(_userService.Register(value));
        }

    }
}
