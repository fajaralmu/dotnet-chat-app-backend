using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAPI.Constollers;
using ChatAPI.Dto;
using ChatAPI.Helper;
using ChatAPI.Models;
using ChatAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    // [Route("api/chat")]
    [ApiController, Authorize]
    public class ChatController : BaseController
    {
        private readonly ChatService _chatService;
        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet, Route("api/chat/{id}")]
        public ActionResult<WebResponse<ChatMessage>> Read(int id)
        {
            return CommonJson(_chatService.Read(id));
        }

        [HttpGet, Route("api/chat/{roomID}")]
        public ActionResult<WebResponse<List<ChatMessage>>> GetRoomChat(int roomID)
        {
            return CommonJson(_chatService.GetByRoomID(roomID));
        }
        [HttpPost]
        [Route("api/chat/room")]
        public ActionResult<WebResponse<ChatMessage>> SendGroup(ChatMessage message)
        {
            return CommonJson(_chatService.SendGroupChat(message, GetLoggedUser()));
        }

        [HttpGet, Route("api/chat/direct/{toUserID}")]
        public ActionResult<WebResponse<List<ChatMessage>>> GetDirectChat(int toUserID)
        {
            return CommonJson(_chatService.GetByToUserID(toUserID, GetLoggedUser()));
        }

        [HttpPost]
        [Route("api/chat/direct")]
        public ActionResult<WebResponse<ChatMessage>> SendDirect(ChatMessage message)
        {
            return CommonJson(_chatService.SendDirectChat(message, GetLoggedUser()));
        }

        [HttpDelete, Route("api/chat/{id}")]
        public ActionResult<WebResponse<ChatMessage>> Delete(int id)
        {
            return CommonJson(_chatService.Delete(id, GetLoggedUser()));
        }

    }
}
