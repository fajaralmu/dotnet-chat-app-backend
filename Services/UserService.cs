using System;
using System.Collections.Generic;
using System.Linq;
using ChatAPI.Models;
using ChatAPI.Context;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Services
{
    public class UserService
    {
        private readonly UserContext _context;
        public UserService(UserContext context)
        {
            _context = context;
        }

        internal IEnumerable<User> getUsers()
        {
            return _context.UserItems.ToList<User>();
        }

        internal User Register(User user)
        {
            user.role = "user";
            user.ID = 0;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.UserItems.Add(user);
            _context.SaveChanges();

            user.Password = null;
            return user;
        }
    }
}