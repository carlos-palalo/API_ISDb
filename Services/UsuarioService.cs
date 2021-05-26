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

namespace API_ISDb.Services
{
    /// <summary>
    /// UsuarioService
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly proyectoContext _context;
        private readonly IConfiguration _config;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        public UsuarioService(proyectoContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Obtengo todos los usuarios
        /// </summary>
        /// <returns></returns>
        public ICollection<Usuario> GetAll()
        {
            return _context.Usuario.ToArray();
        }

        /// <summary>
        /// Obtengo un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Usuario GetUsuario(int id)
        {
            return _context.Usuario.Find(id);
        }

        /// <summary>
        /// Añado un usuario
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
        /// Actualizo un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Boolean PutUsuario(Usuario usuario)
        {
            var v_usuario = _context.Usuario.SingleOrDefault(a => a.IdUsuario == usuario.IdUsuario);
            if (v_usuario != null)
            {
                //usuario.Password = Encrypt.GetSHA256(usuario.Password);
                _context.Entry(v_usuario).CurrentValues.SetValues(usuario);
                _context.Update(v_usuario);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro un usuario
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
        /// Registro de un usuario
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public string register(Usuario _users)
        {
            string response = "";
            //Compruebo existencia de usuario, lo añado y genero el token
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
        /// Compruebo existencial username y el email
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
        /// <summary>
        /// Autentificación de Usuario
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private Usuario AuthenticateUser(Usuario login)
        {
            Usuario user = null;

            //Validate the User Credentials      
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
        /// <summary>
        /// Generación JWT
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSONWebToken(Usuario userInfo)
        {
            // Parámetros almacenados en appsettings.json
            var secretKey = _config.GetSection("Jwt").GetSection("Key").Value;
            var audienceToken = _config.GetSection("Jwt").GetSection("Audience").Value;
            var issuerToken = _config.GetSection("Jwt").GetSection("Issuer").Value;
            var expireTime = _config.GetSection("Jwt").GetSection("Exp_Min").Value;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string tipo;

            tipo = userInfo.Tipo.Equals("admin") ? "admin" : "normal";

            // Parámetros personalizados del token. idUsuario, unique_name y role
            var claimsToken = new List<Claim>
            {
                new Claim("idUsuario", userInfo.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Username),
                new Claim("role", tipo)
            };

            // Creación del token
            var token = new JwtSecurityToken(
                issuer: issuerToken,        // Emisor del token          
                audience: audienceToken,    // Receptor del token
                notBefore: DateTime.Now,    // A partir de este momento, el token es válido
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(expireTime)),  // A partir de este momento, el token deja de ser válido
                signingCredentials: credentials,    // Representa la firma, el id de la firma y el algoritmo de seguridad usado para crear el token
                claims: claimsToken                 // Parámetros personalizados
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
