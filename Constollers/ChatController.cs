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

        /////////////////// group ///////////////

        [HttpGet, Route("api/chat/room/{roomID}")]
        public ActionResult<WebResponse<List<ChatMessage>>> GetRoomChat(int roomID)
        {
            return CommonJson(_chatService.GetByRoomID(roomID));
        }
        [HttpPost]
        [Route("api/chat/room")]
        public ActionResult<WebResponse<ChatMessage>> SendGroup(ChatMessageDto message)
        {
            return CommonJson(_chatService.SendGroupChat(message, GetLoggedUser()));
        }

        //////////////// direct //////////////////

        [HttpGet, Route("api/chat/direct/{fromUserID}")]
        public ActionResult<WebResponse<List<ChatMessage>>> GetDirectChat(int fromUserID)
        {
            return CommonJson(_chatService.GetByFromUserID(fromUserID, GetLoggedUser()));
        }

        [HttpPost]
        [Route("api/chat/direct")]
        public ActionResult<WebResponse<ChatMessage>> SendDirect(ChatMessageDto message)
        {
            return CommonJson(_chatService.SendDirectChat(message, GetLoggedUser()));
        }

        [HttpDelete, Route("api/chat/{id}")]
        public ActionResult<WebResponse<ChatMessage>> Delete(int id)
        {
            return CommonJson(_chatService.Delete(id, GetLoggedUser()));
        }
        [HttpGet, Route("api/chat/partners")]
        public ActionResult<WebResponse<List<User>>> GetPartners()
        {
            return CommonJson(_chatService.GetPartners(GetLoggedUser()));
        }

    }
}
