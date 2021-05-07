using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_ISDb.Models;
using API_ISDb.Interfaces;

namespace API_ISDb.Services
{
    /// <summary>
    /// UserService
    /// </summary>
    public class UserService : IUserService
    {
        private static List<Users> lista = new List<Users>();
        private IConfiguration _config;

        /// <summary>
        /// UserService constructor
        /// </summary>
        /// <param name="config"></param>
        public UserService(IConfiguration config)
        {
            _config = config;
            if (lista.Count == 0)
                lista.Add(new Users("admin", "admin"));
        }

        /// <summary>
        /// Authenticate and generate token
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public string login(Users _users)
        {
            string response = "";
            var user = AuthenticateUser(_users);
            if (user != null)
            {
                response = GenerateJSONWebToken(user);
            }

            return response;
        }

        #region AuthenticateUser
        // Hardcoded the User authentication  
        private Users AuthenticateUser(Users login)
        {
            Users user = null;

            //Validate the User Credentials      
            //Demo Purpose, I have Passed HardCoded User Information   
            foreach (Users x in lista)
            {
                if (x.username == login.username && x.password == login.password)
                {
                    user = x;
                }
            }
            return user;
        }
        #endregion

        #region GenerateJWT  
        // Generate Json Web Token Method  
        private string GenerateJSONWebToken(Users userInfo)
        {
            var secretKey = _config.GetSection("Jwt").GetSection("Key").Value;
            var audienceToken = _config.GetSection("Jwt").GetSection("Audience").Value;
            var issuerToken = _config.GetSection("Jwt").GetSection("Issuer").Value;
            var expireTime = _config.GetSection("Jwt").GetSection("Exp_Min").Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsToken = new List<Claim>
            {
                new Claim("username", userInfo.username),
            };

            var token = new JwtSecurityToken(
                issuer: issuerToken,
                audience: audienceToken,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: credentials,
                claims: claimsToken
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
