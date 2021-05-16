using API_ISDb.Examples;
using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Controllers
{
    /// <summary>
    /// Controller Login
    /// </summary>
    public class LoginController : BaseController
    {
        #region Property  
        /// <summary>  
        /// Property Declaration  
        /// </summary>  
        /// <returns></returns>  
        private IUsuarioService _user;
        #endregion

        #region Contructor Injector  
        /// <summary>  
        /// Constructor Injection to access all methods or simply DI(Dependency Injection)  
        /// </summary>  
        public LoginController(IUsuarioService usuarioService)
        {
            _user = usuarioService;
        }
        #endregion

        #region Login Validation  
        //Login => login/
        /// <summary>
        /// Login method
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Login
        ///     {
        ///         "username": "your-username",
        ///         "password": "your-password"
        ///     }
        /// </remarks>
        /// <response code="200">Login success</response>
        /// <response code="400">Exception. Bad Request</response>
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="404">Login failed</response>
        [AllowAnonymous]
        [HttpPost("login")]
        [Produces("application/json", Type = typeof(string))]
        public IActionResult DoLogin([FromBody] ELogin users)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario usuario = new Usuario();
                    usuario.Username = users.Username;
                    usuario.Password = users.Password;

                    string response = _user.login(usuario);

                    if (!response.Equals(""))
                    {
                        Usuario user = _user.GetCurrentUser(usuario);
                        Program._log.Information("Login successfull");
                        return Ok(new { username = user.Username, tipo = user.Tipo, token = response });
                    }

                    Program._log.Warning("Login Failed");
                    return NotFound("Login Failed. Username or password not found");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Register
        ///     {
        ///         "Username": "name",
        ///         "Password": "pass",
        ///         "Email": "email"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register/")]
        public ActionResult Register([FromBody] ERegister user)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.Username = user.Username;
                usuario.Password = user.Password;
                usuario.Email = user.Email;
                usuario.Tipo = "normal";

                string token = _user.register(usuario);
                return Ok(new { username = usuario.Username, tipo = usuario.Tipo, token = token });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
