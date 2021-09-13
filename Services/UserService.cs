using System;
using System.Collections.Generic;
using System.Linq;
using ChatAPI.Models;
using ChatAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Authentication;

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

        internal User Register(IFormCollection formBody)
        {
            User user = new User();
            user.role = "user";
            user.ID = 0;
            user.Email = formBody["email"];
            user.Name = formBody["name"];
            user.Password = HashPassword(formBody["password"]);
            _context.UserItems.Add(user);
            _context.SaveChanges();

            user.Password = null;
            return user;
        }

        private string HashPassword(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

        internal User Login(IFormCollection value)
        {
            string email = value["email"];
            string password = HashPassword(value["password"]);
            IQueryable<User> users = _context.UserItems.Where<User>(u => u.Email == email);
            if (users.Count() == 0){
                throw new InvalidCredentialException("Email not found");
            }
            User user = users.First();
            if (user.Password != password){
                throw new InvalidCredentialException("Password incorrect");
            }
            return user;
        }
    }
}