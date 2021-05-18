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
        private IGeneroService _genero;
        private IRepartoService _reparto;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serie"></param>
        /// <param name="general"></param>
        /// <param name="genero"></param>
        /// <param name="reparto"></param>
        public GeneralController(IUsuarioService user, ISerieService serie, IGeneralService general, IGeneroService genero, IRepartoService reparto)
        {
            _user = user;
            _serie = serie;
            _general = general;
            _genero = genero;
            _reparto = reparto;
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
        [HttpGet("getserie/{id}")]
        public ActionResult GetSerie(int id)
        {
            return Ok(_general.GetInfoSerie(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getgenero/{id}")]
        public ActionResult GetGeneros(int id)
        {
            return Ok(_genero.GetGeneros(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getreparto/{id}")]
        public ActionResult GetReparto(int id)
        {
            return Ok(_reparto.GetRepartos(id));
        }

        /// <summary>
        /// Devuelve los nombres de las series y su id
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("searchserie/")]
        public ActionResult SearchSerie()
        {
            IEnumerable<SearchSerie> response = _general.SearchSerie();
            if (response.Count() != 0)
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
                //usuario.Password = Encrypt.GetSHA256(user.Password);
                usuario.Email = user.Email;
                usuario.Tipo = user.Tipo;

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
