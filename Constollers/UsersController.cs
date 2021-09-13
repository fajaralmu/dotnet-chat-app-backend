using System.Collections.Generic;
using ChatAPI.Dto;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> Index()
        {
            return Json(
                WebResponse<IEnumerable<User>>.success(_userService.getUsers())
            );
        }
        [HttpPost]
        public ActionResult<User> Register(User user)
        {
            return Json(
                WebResponse<User>.success(_userService.Register(user))
            );
        }

    }
}
