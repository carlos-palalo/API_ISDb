using API_ISDb.Examples;
using API_ISDb.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API_ISDb.Controllers
{
    /// <summary>
    /// LoginController
    /// </summary>
    public class LoginController : BaseController
    {
        private readonly IUsuarioService _user;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="usuarioService"></param>
        public LoginController(IUsuarioService usuarioService)
        {
            _user = usuarioService;
        }

        #region Login Validation  
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
        /// <returns></returns>
        /// <response code="200">Login success</response>
        /// <response code="400">Exception. Bad Request</response>
        /// <response code="404">Login failed</response>
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult DoLogin([FromBody] ELogin users)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario usuario = new Usuario
                    {
                        Username = users.Username,
                        Password = users.Password
                    };

                    // Comprueba si existe el usuario y devuelve el token generado
                    string response = _user.login(usuario);

                    if (!response.Equals(""))
                    {
                        Program._log.Information("Login successfull");
                        return Ok(response);
                    }
                    else
                    {
                        Program._log.Warning("Login Failed");
                        return NotFound("Login Failed. Username or password not found");
                    }
                }
                else
                {
                    Program._log.Warning("Login Failed. Bad Request");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace)
                {
                    StatusCode = 500
                };
                return response;
            }
        }
        #endregion

        /// <summary>
        /// Register method
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
        /// <response code="200">Register success</response>
        /// <response code="400">Exception. Bad Request</response>
        /// <response code="404">Register failed</response>
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpPost("register/")]
        public ActionResult Register([FromBody] ERegister user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario usuario = new Usuario
                    {
                        Username = user.Username,
                        Password = user.Password,
                        Email = user.Email,
                        Tipo = "normal"
                    };

                    // Añade al usuario y devuelve el token generado
                    string token = _user.register(usuario);
                    if (!token.Equals(""))
                    {
                        Program._log.Information("Register successfull");
                        return Ok(token);
                    }
                    else
                    {
                        Program._log.Warning("Register Failed");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Register Failed. Bad Request");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace)
                {
                    StatusCode = 500
                };
                return response;
            }
        }
    }
}
