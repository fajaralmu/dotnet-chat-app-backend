using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ChatAPI.Context;
using ChatAPI.Dto;
using ChatAPI.Helper;
using ChatAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class ChatService : BaseMasterDataService<ChatMessage>
    {
        public static IEqualityComparer<User> CreateEqualityComparer<User>(
            
            Func<User, int> getHashCode, 
            Func<User, User, bool> equals
            ){
            return new DelegatedEqualityComparer<User>(getHashCode, equals);
        }
        public ChatService(ChatAppContext context) : base(context)
        {

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
                Body = messageDto.Body
            };
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
            return GetQuery(chat =>
                            (chat.FromUser.ID == fromUserID && chat.ToUser.ID == user.ID) ||
                            (chat.FromUser.ID == user.ID && chat.ToUser.ID == fromUserID))
                            .OrderBy(chat => chat.CreatedDate)
                            .Include(chat => chat.FromUser)
                            .Include(chat => chat.ToUser)
                            .ToList();

        }

        public ChatMessage Delete(int id, User user)
        {
            return base.Delete(id, chat => chat.FromUserID == user.ID);
        }

        internal List<User> GetPartners(User user)
        {
            List<User> result = Items
                        .Where(chat => (chat.ToUserID == user.ID || chat.FromUserID == user.ID) && chat.Room == null )
                        .Select(chat => (chat.FromUserID == user.ID)? chat.ToUser: chat.FromUser )
                        .AsEnumerable<User>() //<== SQL to DB is executed
                        .Distinct(
                            CreateEqualityComparer<User>(
                                user=>user.GetHashCode(),
                                (user1, user2) => user1.ID == user2.ID
                            )
                        )
                        .ToList();
            return result;
        }

        protected override DbSet<ChatMessage> Items => _context.ChatMessages;
    }
}