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
        private IUserService _userService;
        #endregion

        #region Contructor Injector  
        /// <summary>  
        /// Constructor Injection to access all methods or simply DI(Dependency Injection)  
        /// </summary>  
        public LoginController(IUserService userService)
        {
            _userService = userService;
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
        [HttpPost("")]
        [Produces("application/json", Type = typeof(string))]
        public IActionResult DoLogin([FromBody] Users users)
        {
            try
            {
                string response = _userService.login(users);

                if (!response.Equals(""))
                {
                    Program._log.Information("Login successfull");
                    return Ok(response);
                }

                Program._log.Warning("Login Failed");
                return NotFound("Login Failed. Username or password not found");
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
    }
}
