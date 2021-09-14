using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChatAPI.Context;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class ChatService : BaseMasterDataService<ChatMessage>
    {
        public ChatService(ChatAppContext context) : base(context)
        {
            
        }

        public ChatMessage SendDirectChat(ChatMessage message, User user)
        {
            User toUser = _context.Users.Where(u => u.ID == message.ToUserID).FirstOrDefault();
            if (null == toUser)
            {
                throw new ArgumentException("to user not found");
            }
            message.FromUser = user;
            message.ToUser = toUser;
            return Add(message);
        }

        public ChatMessage SendGroupChat(ChatMessage message, User user)
        {
            ChatRoom room = _context.ChatRooms.Where(room => room.ID == message.RoomID).FirstOrDefault();
            if (null == room){
                throw new ArgumentException("Room not found");
            }
            message.Room = room;
            message.FromUser = user;
            return Add(message);
        }

        public List<ChatMessage> GetByRoomID(int roomID)
        {
            return GetList(chat => chat.RoomID == roomID);
        }
        public List<ChatMessage> GetByToUserID(int toUserID, User user)
        {
            return GetList(chat => 
                            chat.ToUserID == toUserID && 
                            user.ID == chat.FromUserID 
                            );
        }

        public ChatMessage Delete(int id, User user)
        {
            return base.Delete(id, chat=>chat.FromUserID == user.ID);
        }

        protected override DbSet<ChatMessage> Items => _context.ChatMessages;
    }
}