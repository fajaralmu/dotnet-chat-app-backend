using System;
using System.Text;
using System.Threading.Tasks;
using ChatAPI.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using ChatAPI.Models;

namespace ChatAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(
            RequestDelegate next,
            IConfiguration configuration
            ) {
            _next = next; 
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, UserService userService)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();
            User user = null;
            if (token != null)
                user = attachUserToContext(context, userService, token);
                
            if (null != user)
            {
                string generatedToken = userService.GenerateJwtToken(user);
                context.Response.Headers.Add("access-token", generatedToken);
            }

            await _next(context);
        }

        private User attachUserToContext(HttpContext context, UserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == "email").Value;

                // attach user to context on successful jwt validation
                User user = userService.GetByEmail(email);
                context.Items["User"] = user;
                return user;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                return null;
            }
        }
    }
}