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
    /// Controlador al que accede cualquier usuario
    /// </summary>
    public class GeneralController : BaseController
    {
        private IUsuarioService _user;
        private ISerieService _serie;
        private IGeneralService _general;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serie"></param>
        /// <param name="general"></param>
        public GeneralController(IUsuarioService user, ISerieService serie, IGeneralService general)
        {
            _user = user;
            _serie = serie;
            _general = general;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getseries/")]
        public ActionResult GetAllSeries()
        {
            return Ok(_serie.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getinfoserie/{id}")]
        public ActionResult GetInfoSeries(int id)
        {
            return Ok(_general.GetInfoSerie(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("searchserie/")]
        public ActionResult SearchSerie([FromBody] string serie)
        {
            ICollection<Serie> response = _general.SearchSerie(serie);
            if (response.Count()!=0)
            {
                return Ok(response);
            }
            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getusuario/{id}")]
        public ActionResult GetUsuario(int id)
        {
            var usuarios = _user.GetUsuario(id);
            if (usuarios != null)
                return Ok(usuarios);
            else
                return NotFound();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Usuario
        ///     {
        ///         "IdUsuario": 0,
        ///         "Username": "name",
        ///         "Password": "pass",
        ///         "Email": "email",
        ///         "Tipo": "tipo",
        ///         "Genero": "genero",
        ///         "Pais": "pais",
        ///         "Nacimiento": "2020-12-25",
        ///         "FotoPerfil": "url"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("putusuario/")]
        public ActionResult PutUsuario([FromBody] EUUsuario user)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.IdUsuario = user.IdUsuario;
                usuario.Username = user.Username;
                usuario.Password = Encrypt.GetSHA256(user.Password);
                usuario.Email = user.Email;
                usuario.Tipo = user.Tipo;
                usuario.Genero = user.Genero;
                usuario.Pais = user.Pais;
                usuario.Nacimiento = user.Nacimiento;
                usuario.FotoPerfil = user.FotoPerfil;

                answer = _user.PutUsuario(usuario);
                if (answer)
                    return Ok();
                else
                    return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("deleteusuario/{id}")]
        public ActionResult DeleteUsuario(int id)
        {
            Boolean answer = _user.DeleteUsuario(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
    }
}
