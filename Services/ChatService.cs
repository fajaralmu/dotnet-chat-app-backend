using System;
using System.Collections.Generic;
using System.Linq;
using ChatAPI.Context;
using ChatAPI.Dto;
using ChatAPI.Helper;
using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class ChatService : BaseMasterDataService<ChatMessage>
    {
        WebsocketService _websocketService;
        UserService _userService;
        private readonly IHubContext<ChatHub> _chatHub;
        public ChatService(
            ChatAppContext context, 
            WebsocketService websocketService,
            IHubContext<ChatHub> chatHub,
            UserService userService
            ) : base(context)
        {
            _websocketService   = websocketService;
            _userService        = userService;
            _chatHub            = chatHub;
        }

        public ChatMessage SendDirectChat(ChatMessageDto messageDto, User user)
        {
            if (messageDto.ToUserID == user.ID)
            {
                throw new AccessViolationException("not allowed");
            }
            User toUser = _context.Users.Where(u => u.ID == messageDto.ToUserID).FirstOrDefault();
            if (null == toUser)
            {
                throw new ArgumentException("to user not found");
            }
            ChatMessage message = new ChatMessage()
            {
                FromUser = user,
                ToUser = toUser,
                Body = messageDto.Body,
                FromUserID = user.ID,
            };
            Console.WriteLine("====== will send websocket =======");
            
            _chatHub.Clients.All.SendAsync("ReceiveMessage", toUser.Email, 
                WebResponse<ChatMessage>.SuccessResponse(message));
            _websocketService.SendToAll("chat/"+toUser.ID, message);
            return Add(message);
        }

        public ChatMessage SendGroupChat(ChatMessageDto messageDto, User user)
        {
            ChatRoom room = _context.ChatRooms.Where(room => room.ID == messageDto.RoomID).FirstOrDefault();
            if (null == room)
            {
                throw new ArgumentException("Room not found");
            }
            ChatMessage message = new ChatMessage()
            {
                FromUser = user,
                Room = room,
                Body = messageDto.Body
            };
            return Add(message);
        }

        public List<ChatMessage> GetByRoomID(int roomID)
        {
            return GetList(chat => chat.RoomID == roomID);
        }
        public List<ChatMessage> GetByFromUserID(int fromUserID, User user)
        {
            return GetQuery (chat =>
                                (chat.FromUser.ID == fromUserID && chat.ToUser.ID == user.ID) ||
                                (chat.FromUser.ID == user.ID && chat.ToUser.ID == fromUserID)
                            )
                            .OrderBy(chat => chat.CreatedDate)
                            .Include(chat => chat.FromUser)
                            .Include(chat => chat.ToUser)
                            .ToList();

        }

        public ChatMessage Delete(int id, User user)
        {
            return base.Delete(id, chat => chat.FromUserID == user.ID);
        }

        internal List<User> GetPartners(User user, string searchName = null)
        {
            List<User> result = Items
                        .Where(chat => 
                            (chat.ToUserID == user.ID || chat.FromUserID == user.ID) && chat.Room == null ||
                            (searchName == null ? false : chat.ToUser.Name.ToLower().Contains(searchName.ToLower()))
                            )
                        .Select(chat => (chat.FromUserID == user.ID)? chat.ToUser: chat.FromUser )
                        .AsEnumerable<User>() //<== SQL to DB is executed
                        .Distinct(
                            CreateEqualityComparer<User>(
                                user=>user.GetHashCode(),
                                (user1, user2) => user1.ID == user2.ID
                            )
                        )
                        .ToList();

            if (null != searchName) {
                List<User> users = _userService.GetByNameLike(user, searchName);
                result.AddRange(users);
                return result.Distinct().ToList();
            }
            return result;
        }

        private bool ChatPartnerFilter(ChatMessage chat, User user, string name = null)
        {
            bool hasChatHistoryWithUser = (chat.ToUserID == user.ID || chat.FromUserID == user.ID) && chat.Room == null;
            if (name != null) {
                return hasChatHistoryWithUser && chat.FromUser.Name.ToLower().Contains(name.ToLower());
            }
            return hasChatHistoryWithUser;
        }

        protected override DbSet<ChatMessage> Items => _context.ChatMessages;

        public IHubContext<ChatHub> ChatHub => _chatHub;
    }
}