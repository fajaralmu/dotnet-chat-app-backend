using System;
using System.Linq;
using ChatAPI.Context;
using ChatAPI.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatAPI.Services
{
    public class SettingService:BaseMasterDataService<ApplicationProfile>
    {
        private readonly IHubContext<ChatHub> _chatHub;
        public SettingService(ChatAppContext context, IHubContext<ChatHub> chatHubContext) : base(context)
        {
            _chatHub = chatHubContext;
        }

        private static readonly Random _random = new Random();
        protected override DbSet<ApplicationProfile> Items => _context.ApplicationProfiles;

        internal ApplicationProfile GetProfile()
        {
            ApplicationProfile model = Items.FirstOrDefault();
            if (null == model){
                model = Add(ApplicationProfile.Default);
            }
            model.RequestID = (100000 + _random.Next(100000)).ToString();
            return model;
        }
    }
}