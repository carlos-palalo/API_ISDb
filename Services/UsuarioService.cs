using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private proyectoContext _context;
        private IConfiguration _config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UsuarioService(proyectoContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Usuario> GetAll()
        {
            return _context.Usuario.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario GetUsuario(int id)
        {
            return _context.Usuario.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Usuario PostUsuario(Usuario usuario)
        {
            usuario.Password = Encrypt.GetSHA256(usuario.Password);
            usuario.Tipo = "normal";
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Boolean PutUsuario(Usuario usuario)
        {
            var v_usuario = _context.Usuario.SingleOrDefault(a => a.IdUsuario == usuario.IdUsuario);
            if (v_usuario != null)
            {
                usuario.Password = Encrypt.GetSHA256(usuario.Password);
                _context.Entry(v_usuario).CurrentValues.SetValues(usuario);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteUsuario(int id)
        {
            var usuarios = _context.Usuario.SingleOrDefault(a => a.IdUsuario == id);
            if (usuarios != null)
            {
                _context.Usuario.Remove(usuarios);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Authenticate and generate token
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public string login(Usuario _users)
        {
            string response = "";
            var user = AuthenticateUser(_users);
            if (user != null)
            {
                response = GenerateJSONWebToken(user);
            }

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public string register(Usuario _users)
        {
            string response = "";
            Usuario checkuser = CheckUser(_users);
            if (checkuser == null)
            {
                var user = PostUsuario(_users);
                if (user != null)
                {
                    response = GenerateJSONWebToken(user);
                }
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public Usuario CheckUser(Usuario _users)
        {
            ICollection<Usuario> users = GetAll();
            foreach (Usuario user in users)
            {
                if (user.Email.Equals(_users.Email) || user.Username.Equals(_users.Username))
                {
                    return user;
                }
            }

            return null;
        }

        #region AuthenticateUser
        // Hardcoded the User authentication  
        private Usuario AuthenticateUser(Usuario login)
        {
            Usuario user = null;

            //Validate the User Credentials      
            //Demo Purpose, I have Passed HardCoded User Information   

            foreach (Usuario x in _context.Usuario.ToArray())
            {
                if (x.Username == login.Username && x.Password == Encrypt.GetSHA256(login.Password))
                {
                    user = x;
                }
            }
            return user;
        }
        #endregion

        #region GenerateJWT  
        // Generate Json Web Token Method  
        private string GenerateJSONWebToken(Usuario userInfo)
        {
            var secretKey = _config.GetSection("Jwt").GetSection("Key").Value;
            var audienceToken = _config.GetSection("Jwt").GetSection("Audience").Value;
            var issuerToken = _config.GetSection("Jwt").GetSection("Issuer").Value;
            var expireTime = _config.GetSection("Jwt").GetSection("Exp_Min").Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string tipo;

            tipo = userInfo.Tipo.Equals("admin") ? "admins" : "normal";

            var claimsToken = new List<Claim>
            {
                new Claim("idUsuario", userInfo.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Username),
                new Claim(ClaimTypes.Role, tipo)
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
