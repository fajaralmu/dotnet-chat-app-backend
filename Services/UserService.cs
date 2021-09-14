using System;
using System.Collections.Generic;
using System.Linq;
using ChatAPI.Models;
using ChatAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Security.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Services
{
    public class UserService : BaseMasterDataService<User>
    {
        private readonly IConfiguration _configuration;

        protected override DbSet<User> Items => _context.Users;

        public UserService(ChatAppContext context, IConfiguration configuration) :base(context)
        { 
            _configuration = configuration;
        }

        internal IEnumerable<User> getUsers()
        {
            return _context.Users.ToList<User>();
        }

        internal User Register(IFormCollection formBody)
        {
            User user = new User();
            user.Role = "user";
            user.ID = 0;
            user.Email = formBody["email"];
            user.Name = formBody["name"];
            user.Password = HashPassword(formBody["password"]);
            Add(user);

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
            string password = value["password"];
            IQueryable<User> users = _context.Users.Where<User>(u => u.Email == email);
            if (users.Count() == 0){
                throw new InvalidCredentialException($"Email :{email} not found");
            }
            User user = users.First();
            if (!BCrypt.Net.BCrypt.Verify(password,  user.Password)) {
                throw new InvalidCredentialException("Password incorrect");
            }
            user.Token = GenerateJwtToken(user);
            return user;
        }

        internal User GetByEmail(string email)
        {
            return _context.Users.Where(u=>u.Email == email).FirstOrDefault();
        }

        public string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", user.Email) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}