using API_ISDb.Examples;
using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Controllers
{
    /// <summary>
    /// GeneralController
    /// </summary>
    public class GeneralController : BaseController
    {
        private readonly IUsuarioService _user;
        private readonly ISerieService _serie;
        private readonly IGeneralService _general;
        private readonly IReviewService _review;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="user"></param>
        /// <param name="serie"></param>
        /// <param name="general"></param>
        /// <param name="review"></param>
        public GeneralController(IUsuarioService user, ISerieService serie, IGeneralService general, IReviewService review)
        {
            _user = user;
            _serie = serie;
            _general = general;
            _review = review;
        }

        /// <summary>
        /// Obtención de todas las series
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpGet("getseries/")]
        public ActionResult GetAllSeries()
        {
            try
            {
                return Ok(_serie.GetAll());
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

        /// <summary>
        /// Obtención de la información de una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpGet("getserie/{id}")]
        public ActionResult GetSerie(int id)
        {
            try
            {
                return Ok(_general.GetInfoSerie(id));
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

        /// <summary>
        /// Obtención del id y titulo de las series para el SearchBox
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="404">Tabla Serie vacía</response>        
        /// <response code="500">Internal Server Error</response>
        [AllowAnonymous]
        [HttpGet("searchserie/")]
        public ActionResult SearchSerie()
        {
            try
            {
                IEnumerable<SearchSerie> response = _general.SearchSerie();
                if (response.Count() != 0)
                {
                    Program._log.Information("Éxito al obtener id y titulo de las series");
                    return Ok(response);
                }
                else
                {
                    Program._log.Warning("Tabla Serie vacía");
                    return NotFound();
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

        /// <summary>
        /// Obtención de la información de un Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("getusuario/{id}")]
        public ActionResult GetUsuario(int id)
        {
            try
            {
                var usuarios = _user.GetUsuario(id);
                if (usuarios != null)
                {
                    Program._log.Information("Éxito al obtener información de Usuario con id " + id);
                    return Ok(usuarios);
                }
                else
                {
                    Program._log.Warning("Usuario con id " + id + " no encontrado");
                    return NotFound();
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

        /*
        /// <summary>
        /// Actualización de los datos del usuario
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
            try
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
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }
        */

        /// <summary>
        /// Actualización de password del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("updatepass")]
        public ActionResult UpdatePassword(EUPassword user)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Usuario usuario = _user.GetUsuario(Convert.ToInt32(user.idUser));
                    usuario.Password = Encrypt.GetSHA256(user.Password);

                    answer = _user.PutUsuario(usuario);
                    if (answer)
                    {
                        Program._log.Information("Actualización de contraseña con éxito del usuario " + user.idUser);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar contraseña. Usuario Not Found");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar contraseña. Bad Request");
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

        /// <summary>
        /// Actualización de la información del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("updateinfo")]
        public ActionResult UpdateInfUser(EUInfo user)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Usuario usuario = _user.GetUsuario(Convert.ToInt32(user.IdUsuario));
                    usuario.Username = user.Username;
                    usuario.Email = user.Email;

                    answer = _user.PutUsuario(usuario);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar información del usuario con id " + user.IdUsuario);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar información del usuario. Not Found");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar información del usuario. Bad Request");
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

        /// <summary>
        /// Creación de una nueva review
        /// </summary>
        /// <param name="rev"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="404">Error al crear Review</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost("postreview/")]
        public ActionResult PostReview([FromBody] EReview rev)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Review review = new Review
                    {
                        //review.IdReview = rev.IdReview;
                        Titulo = rev.Titulo,
                        Descripcion = rev.Descripcion,
                        Puntuacion = Convert.ToInt32(rev.Puntuacion),
                        Fecha = DateTime.Now,
                        UsuarioIdUsuario = Convert.ToInt32(rev.UsuarioIdUsuario),
                        SerieIdSerie = Convert.ToInt32(rev.SerieIdSerie)
                    };

                    var reviews = _review.PostReview(review);
                    if (reviews != null)
                    {
                        Program._log.Information("Éxito al crear una nueva review");
                        return Ok(reviews);
                    }
                    else
                    {
                        Program._log.Warning("Error al crear una nueva review");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al crear una nueav review. Bad Request");
                    return NotFound();
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
