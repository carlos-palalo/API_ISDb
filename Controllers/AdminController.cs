using API_ISDb.Examples;
using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static API_ISDb.Models.RequestSeries;

namespace API_ISDb.Controllers
{
    /// <summary>
    /// Controllador al que accede sólo el Admin
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : BaseController
    {
        private ISerieService _serie;
        private IGeneroService _genero;
        private IRepartoService _reparto;
        private IRepartoRoleService _repartoRole;
        private IReviewService _review;
        private IRoleService _role;
        private ISerieGeneroService _serieGenero;
        private ISerieRepartoService _serieReparto;
        private IUsuarioService _usuario;
        private IImdbService _imdb;

        /// <summary>
        /// Admin controller constructor
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <param name="reparto"></param>
        /// <param name="repartoRole"></param>
        /// <param name="review"></param>
        /// <param name="role"></param>
        /// <param name="serieGenero"></param>
        /// <param name="serieReparto"></param>
        /// <param name="usuario"></param>
        /// <param name="imdb"></param>
        public AdminController(ISerieService serie, IGeneroService genero, IRepartoService reparto,
                               IRepartoRoleService repartoRole, IReviewService review, IRoleService role,
                               ISerieGeneroService serieGenero, ISerieRepartoService serieReparto,
                               IUsuarioService usuario, IImdbService imdb)
        {
            _serie = serie;
            _genero = genero;
            _reparto = reparto;
            _repartoRole = repartoRole;
            _review = review;
            _role = role;
            _serieGenero = serieGenero;
            _serieReparto = serieReparto;
            _usuario = usuario;
            _imdb = imdb;
        }


        #region GenerateBBDD
        /// <summary>
        /// Hace peticiones a la API de IMDb e inserta los datos obtenidos en la BBDD
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("generatebbdd/")]
        [AllowAnonymous]
        public ActionResult GenerateBBDD()
        {
            try
            {
                bool response = _imdb.GenerateBBDD();
                if (response)
                {
                    Program._log.Information("Éxito en la inserción de datos de IMDb en la BBDD.");
                    return Ok();
                }
                else
                {
                    Program._log.Error("Error en la inserción de datos de IMDb en la BBDD");
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
        #endregion

        #region Series 
        /// <summary>
        /// Obtiene todas las series de la BBDD
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getseries/")]
        public ActionResult GetSeries()
        {
            try
            {
                return Ok(_serie.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de los datos de una serie en concreto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Serie Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getserie/{id}")]
        public ActionResult GetSerie(int id)
        {
            try
            {
                var series = _serie.GetSerie(id);
                if (series != null)
                {
                    Program._log.Information("Éxito al buscar serie con id " + id);
                    return Ok(series);
                }
                else
                {
                    Program._log.Warning("Serie con id " + id + " no encontrada");
                    return NotFound();
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

        /// <summary>
        /// Inserción de una serie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Serie
        ///     {
        ///         "Titulo": "string",
        ///         "Poster": "string",
        ///         "Year": 0,
        ///         "Sinopsis": "string",
        ///         "Trailer": "string"
        ///     }
        /// </remarks>
        /// <param name="ser"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postserie/")]
        public ActionResult PostSerie([FromBody] ESerie ser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Serie serie = new Serie();
                    serie.Titulo = ser.Titulo;
                    serie.Poster = ser.Poster;
                    serie.Year = Convert.ToInt32(ser.Year);
                    serie.Sinopsis = ser.Sinopsis;
                    serie.Trailer = ser.Trailer;

                    var series = _serie.PostSerie(serie);
                    if (series != null)
                    {
                        Program._log.Information("Éxito al crear una serie");
                        return Ok(series);
                    }
                    else
                    {
                        Program._log.Warning("Error al crear una serie");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al crear una serie. Bad Request");
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

        /// <summary>
        /// Actualización de una serie
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Serie
        ///     {
        ///         "IdSerie": 0,
        ///         "Titulo": "title",
        ///         "Poster": "url",
        ///         "Year": 0,
        ///         "Sinopsis": "string",
        ///         "Trailer": "url"
        ///     }
        /// </remarks>
        /// <param name="ser"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Serie Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putserie/")]
        public ActionResult PutSerie([FromBody] EUSerie ser)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Serie serie = new Serie();
                    serie.IdSerie = Convert.ToInt32(ser.IdSerie);
                    serie.Titulo = ser.Titulo;
                    serie.Poster = ser.Poster;
                    serie.Year = Convert.ToInt32(ser.Year);
                    serie.Sinopsis = ser.Sinopsis;
                    serie.Trailer = ser.Trailer;

                    answer = _serie.PutSerie(serie);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de la serie " + serie.Titulo);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de la serie " + serie.Titulo);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos de la serie. Bad Request");
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

        /// <summary>
        /// Borrado de una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Serie Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleteserie/{id}")]
        public ActionResult DeleteSerie(int id)
        {
            try
            {
                Boolean answer = _serie.DeleteSerie(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar serie con id " + id);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar serie con id " + id);
                    return NotFound();
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
        #endregion

        #region Usuario 
        /// <summary>
        /// Obtención de todos los usuarios
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getusuarios/")]
        public ActionResult GetUsuarios()
        {
            try
            {
                return Ok(_usuario.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de la información de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getusuario/{id}")]
        public ActionResult GetUsuario(int id)
        {
            try
            {
                var usuarios = _usuario.GetUsuario(id);
                if (usuarios != null)
                {
                    Program._log.Information("Éxito al obtener información del usuario con id " + id);
                    return Ok(usuarios);
                }
                else
                {
                    Program._log.Warning("Error al obtener información del usuario con id " + id);
                    return NotFound();
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

        /// <summary>
        /// Inserción de un usuario
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Usuario
        ///     {
        ///         "Username": "name",
        ///         "Password": "pass",
        ///         "Email": "email"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al crear usuario</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postusuario/")]
        public ActionResult PostUsuario([FromBody] EUsuario user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario usuario = new Usuario();
                    usuario.Username = user.Username;
                    usuario.Password = user.Password;
                    usuario.Email = user.Email;

                    var usuarios = _usuario.PostUsuario(usuario);
                    if (usuarios != null)
                    {
                        Program._log.Information("Éxito al crear usuario");
                        return Ok(usuarios);
                    }
                    else
                    {
                        Program._log.Warning("Error al crear usuario");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al crear usuario. Bad Request");
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

        /// <summary>
        /// Actualización de un usuario
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Usuario
        ///     {
        ///         "IdUsuario": 1,
        ///         "Username": "username",
        ///         "Password": "password",
        ///         "Email": "email",
        ///         "Tipo": "normal/admin"
        ///     }
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putusuario/")]
        public ActionResult PutUsuario([FromBody] EUUsuario user)
        {
            try
            {
                //Console.WriteLine(user.Email);
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Usuario usuario = _usuario.GetUsuario(Convert.ToInt32(user.IdUsuario));
                    usuario.IdUsuario = user.IdUsuario;
                    usuario.Username = user.Username;
                    //usuario.Password = Encrypt.GetSHA256(user.Password);
                    usuario.Email = user.Email;
                    usuario.Tipo = user.Tipo;

                    answer = _usuario.PutUsuario(usuario);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos del usuario " + user.Username);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos del usuario " + user.Username);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos del usuario. Bad Request");
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

        /// <summary>
        /// Borrado de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Usuario Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleteusuario/{id}")]
        public ActionResult DeleteUsuario(int id)
        {
            try
            {
                Boolean answer = _usuario.DeleteUsuario(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar el usuario con id " + id);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar el usuario con id " + id);
                    return NotFound();
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
        #endregion

        #region Review 
        /// <summary>
        /// Obtención de todas las reviews
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getreviews/")]
        public ActionResult GetReviews()
        {
            try
            {
                return Ok(_review.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de los datos de una review en concreto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Review Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getreview/{id}")]
        public ActionResult GetReview(int id)
        {
            try
            {
                var reviews = _review.GetReview(id);
                if (reviews != null)
                {
                    Program._log.Information("Éxito al obtener datos de la review con id " + id);
                    return Ok(reviews);
                }
                else
                {
                    Program._log.Warning("Error al obtener datos de la review con id " + id);
                    return NotFound();
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

        /// <summary>
        /// Creación de una review
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Review
        ///     {
        ///         "Titulo": "title",
        ///         "Descripcion": "string",
        ///         "Puntuacion": "0",
        ///         "UsuarioIdUsuario": "0",
        ///         "SerieIdSerie": "0"
        ///     }
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al crear una review</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postreview/")]
        public ActionResult PostReview([FromBody] EReview rev)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Review review = new Review();
                    //review.IdReview = rev.IdReview;
                    review.Titulo = rev.Titulo;
                    review.Descripcion = rev.Descripcion;
                    review.Puntuacion = Convert.ToInt32(rev.Puntuacion);
                    review.Fecha = DateTime.Now;
                    review.UsuarioIdUsuario = Convert.ToInt32(rev.UsuarioIdUsuario);
                    review.SerieIdSerie = Convert.ToInt32(rev.SerieIdSerie);

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
                    Program._log.Warning("Error al crear una nueva review. Bad Request");
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

        /// <summary>
        /// Actualización de datos de una review
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Review
        ///     {
        ///         "IdReview": "0",
        ///         "Titulo": "title",
        ///         "Descripcion": "string",
        ///         "Puntuacion": "0",
        ///         "Fecha": "2020-12-21",
        ///         "UsuarioIdUsuario": "0",
        ///         "SerieIdSerie": "0"
        ///     }
        /// </remarks>
        /// <param name="rev"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Review Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putreview/")]
        public ActionResult PutReview([FromBody] EUReview rev)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Review review = new Review();
                    review.IdReview = Convert.ToInt32(rev.IdReview);
                    review.Titulo = rev.Titulo;
                    review.Descripcion = rev.Descripcion;
                    review.Puntuacion = Convert.ToInt32(rev.Puntuacion);
                    review.Fecha = rev.Fecha;
                    review.UsuarioIdUsuario = Convert.ToInt32(rev.UsuarioIdUsuario);
                    review.SerieIdSerie = Convert.ToInt32(rev.SerieIdSerie);

                    answer = _review.PutReview(review);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar los datos de la review " + review.IdReview);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar los datos de la review " + rev.IdReview);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar los datos de una review. Bad Request");
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

        /// <summary>
        /// Borrado de una review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Review Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deletereview/{id}")]
        public ActionResult DeleteReview(int id)
        {
            try
            {
                Boolean answer = _review.DeleteReview(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar review con id " + id);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar review con id " + id);
                    return NotFound();
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
        #endregion

        #region Genero 
        /// <summary>
        /// Obtiene todos los géneros
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getgeneros/")]
        public ActionResult GetGeneros()
        {
            try
            {
                return Ok(_genero.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtiene información de un genero
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Genero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getgenero/{id}")]
        public ActionResult GetGenero(int id)
        {
            try
            {
                var generos = _genero.GetGenero(id);
                if (generos != null)
                {
                    Program._log.Information("Éxito al obtener información del género con id " + id);
                    return Ok(generos);
                }
                else
                {
                    Program._log.Warning("Genero con id " + id + " no encontrado");
                    return NotFound();
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

        /// <summary>
        /// Inserción de un género nuevo
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Genero
        ///     {
        ///         "Nombre": "name"
        ///     }
        /// </remarks>
        /// <param name="gen"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al crear Genero</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postgenero/")]
        public ActionResult PostGenero([FromBody] EGenero gen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Genero genero = new Genero();
                    //genero.IdGenero = gen.IdGenero;
                    genero.Nombre = gen.Nombre;

                    var generos = _genero.PostGenero(genero);
                    if (generos != null)
                    {
                        Program._log.Information("Éxito al crear género nuevo");
                        return Ok(generos);
                    }
                    else
                    {
                        Program._log.Warning("Error al crear un género nuevo");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al crear un género nuevo. Bad Request");
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

        /// <summary>
        /// Actualización datos de un género
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Genero
        ///     {
        ///         "IdGenero": 0,
        ///         "Nombre": "name"
        ///     }
        /// </remarks>
        /// <param name="gen"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Genero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putgenero/")]
        public ActionResult PutGenero([FromBody] EUGenero gen)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Genero genero = new Genero();
                    genero.IdGenero = gen.IdGenero;
                    genero.Nombre = gen.Nombre;

                    answer = _genero.PutGenero(genero);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos del género con id " + gen.IdGenero);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Information("Género con id " + gen.IdGenero + " no encontrado");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos del genero. Bad Request");
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

        /// <summary>
        /// Borrado de un género
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Genero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deletegenero/{id}")]
        public ActionResult DeleteGenero(int id)
        {
            try
            {
                Boolean answer = _genero.DeleteGenero(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar genero con id " + id);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Genero con id " + id + " no encontrado");
                    return NotFound();
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
        #endregion

        #region SerieGenero 
        /// <summary>
        /// Obtención de todas las filas de SerieGenero
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getseriegeneros/")]
        public ActionResult GetSerieGenero()
        {
            try
            {
                return Ok(_serieGenero.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de una fila de SerieGenero
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieGenero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getseriegenero/{serie}/{genero}")]
        public ActionResult GetSerieGenero(int serie, int genero)
        {
            try
            {
                var seriegeneros = _serieGenero.GetSerieGenero(serie, genero);
                if (seriegeneros != null)
                {
                    Program._log.Information("Éxito al buscar fila SerieGenero con idSerie " + serie + " e idGenero " + genero);
                    return Ok(seriegeneros);
                }
                else
                {
                    Program._log.Warning("Error al buscar SerieGenero con idSerie " + serie + " e idGenero " + genero);
                    return NotFound();
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

        /// <summary>
        /// Inserción de fila en SerieGenero
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /SerieGenero
        ///     {
        ///         "GeneroIdGenero": 0,
        ///         "SerieIdSerie": 0
        ///     }
        /// </remarks>
        /// <param name="sergen"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al crear SerieGenero</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postseriegenero/")]
        public ActionResult PostSerieGenero([FromBody] ESerieGenero sergen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SerieGenero seriegenero = new SerieGenero();
                    seriegenero.GeneroIdGenero = Convert.ToInt32(sergen.GeneroIdGenero);
                    seriegenero.SerieIdSerie = Convert.ToInt32(sergen.SerieIdSerie);

                    var serieGeneros = _serieGenero.PostSerieGenero(seriegenero);
                    if (serieGeneros != null)
                    {
                        Program._log.Information("Éxito al insertar fila en SerieGenero");
                        return Ok(serieGeneros);
                    }
                    else
                    {
                        Program._log.Warning("Error al insertar fila en SerieGenero");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al insertar fila en SerieGenero. Bad Request");
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

        /// <summary>
        /// Actualización de fila de SerieGenero
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /SerieGenero
        ///     {
        ///         "GeneroIdGenero": 0,
        ///         "SerieIdSerie": 0
        ///     }
        /// </remarks>
        /// <param name="sergen"></param>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieGenero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putseriegenero/{serie}/{genero}")]
        public ActionResult PutSerieGenero([FromBody] ESerieGenero sergen, int serie, int genero)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    SerieGenero seriegenero = new SerieGenero();
                    seriegenero.GeneroIdGenero = Convert.ToInt32(sergen.GeneroIdGenero);
                    seriegenero.SerieIdSerie = Convert.ToInt32(sergen.SerieIdSerie);

                    answer = _serieGenero.PutSerieGenero(seriegenero, serie, genero);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de SerieGenero con idSerie " + serie + " e idGenero " + genero);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de SerieGenero con idSerie " + serie + " e idGenero " + genero);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos de SerieGenero. Bad Request");
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

        /// <summary>
        /// Borrado de SerieGenero
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieGenero Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleteseriegenero/{serie}/{genero}")]
        public ActionResult DeleteSerieGenero(int serie, int genero)
        {
            try
            {
                Boolean answer = _serieGenero.DeleteSerieGenero(serie, genero);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar SerieGenero con idSerie " + serie + " e idGenero " + genero);
                    return Ok();
                }
                else
                {
                    Program._log.Information("Error al borrar SerieGenero con idSerie " + serie + " e idGenero " + genero);
                    return NotFound();
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
        #endregion

        #region Reparto
        /// <summary>
        /// Obtención de todo el reparto de una serie
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getrepartos/{serie}")]
        public ActionResult GetRepartos(int serie)
        {
            try
            {
                return Ok(_reparto.GetRepartos(serie));
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de todas las filas de Reparto. Puede tardar.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getrepartosall/")]
        public ActionResult GetRepartosAll()
        {
            try
            {
                return Ok(_reparto.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de una fila de Reparto en concreto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Reparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getreparto/{id}")]
        public ActionResult GetReparto(int id)
        {
            try
            {
                var repartos = _reparto.GetReparto(id);
                if (repartos != null)
                {
                    Program._log.Information("Éxito al buscar reparto con id " + id);
                    return Ok(repartos);
                }
                else
                {
                    Program._log.Warning("Error al buscar reparto con id " + id);
                    return NotFound();
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

        /// <summary>
        /// Inserción de una fila en Reparto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Reparto
        ///     {
        ///         "Name": "name",
        ///         "Foto": "url"
        ///     }
        /// </remarks>
        /// <param name="rep"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al insertar fila en Reparto</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postreparto/")]
        public ActionResult PostReparto([FromBody] EReparto rep)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Reparto reparto = new Reparto();
                    //reparto.IdReparto = rep.IdReparto;
                    reparto.Name = rep.Name;
                    reparto.Foto = rep.Foto;

                    var repartos = _reparto.PostReparto(reparto);
                    if (repartos != null)
                    {
                        Program._log.Information("Éxito al insertar un nuevo Reparto");
                        return Ok(repartos);
                    }
                    else
                    {
                        Program._log.Warning("Error al insertar un nuevo Reparto");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al insertar un nuevo Reparto. Bad Request");
                    return NotFound();
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

        /// <summary>
        /// Actualización de datos de Reparto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Reparto
        ///     {
        ///         "IdReparto": 0,
        ///         "Name": "name",
        ///         "Foto": "url"
        ///     }
        /// </remarks>
        /// <param name="rep"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Reparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putreparto/")]
        public ActionResult PutReparto([FromBody] EUReparto rep)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Reparto reparto = new Reparto();
                    reparto.IdReparto = rep.IdReparto;
                    reparto.Name = rep.Name;
                    reparto.Foto = rep.Foto;

                    answer = _reparto.PutReparto(reparto);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de Reparto con id " + rep.IdReparto);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de Reparto con id " + rep.IdReparto);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos de Reparto. Bad Request");
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

        /// <summary>
        /// Borrado de una fila de Reparto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Reparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deletereparto/{id}")]
        public ActionResult DeleteReparto(int id)
        {
            try
            {
                Boolean answer = _reparto.DeleteReparto(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar Reparto con id " + id);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar Reparto con id " + id);
                    return NotFound();
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
        #endregion

        #region Role 
        /// <summary>
        /// Obtención de todos los roles
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getroles/")]
        public ActionResult GetRoles()
        {
            try
            {
                return Ok(_role.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de un role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Role Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getrole/{id}")]
        public ActionResult GetRole(int id)
        {
            try
            {
                var roles = _role.GetRole(id);
                if (roles != null)
                {
                    Program._log.Information("Éxito al buscar role con id " + id);
                    return Ok(roles);
                }
                else
                {
                    Program._log.Warning("Error al buscar role con id " + id);
                    return NotFound();
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

        /// <summary>
        /// Inserción de Role nuevo
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Role
        ///     {
        ///         "Nombre": "name"
        ///     }
        /// </remarks>
        /// <param name="rol"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al insertar role nuevo</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postrole/")]
        public ActionResult PostRole([FromBody] ERole rol)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Role role = new Role();
                    //role.IdRole = rol.IdRole;
                    role.Nombre = rol.Nombre;

                    var roles = _role.PostRole(role);
                    if (roles != null)
                    {
                        Program._log.Information("Éxito al insertar role");
                        return Ok(roles);
                    }
                    else
                    {
                        Program._log.Warning("Error al insertar role");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al insertar role. Bad Request");
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

        /// <summary>
        /// Actualización de datos de role
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /Role
        ///     {
        ///         "IdRole": 0,
        ///         "Nombre": "name"
        ///     }
        /// </remarks>
        /// <param name="rol"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Role Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putrole/")]
        public ActionResult PutRole([FromBody] EURole rol)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    Role role = new Role();
                    role.IdRole = rol.IdRole;
                    role.Nombre = rol.Nombre;

                    answer = _role.PutRole(role);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de role");
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de role");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos de role. Bad Request");
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

        /// <summary>
        /// Borrado de role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Role Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleterole/{id}")]
        public ActionResult DeleteRole(int id)
        {
            try
            {
                Boolean answer = _role.DeleteRole(id);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar role");
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar role");
                    return NotFound();
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
        #endregion

        #region RepartoRole 
        /// <summary>
        /// Obtención de todos los datos de RepartoRole
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getrepartoroles/")]
        public ActionResult GetRepartoRoles()
        {
            try
            {
                return Ok(_repartoRole.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de una fila de RepartoRole
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">RepartoRole Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getrepartorole/{reparto}/{role}")]
        public ActionResult GetRepartoRole(int reparto, int role)
        {
            try
            {
                var repartoRole = _repartoRole.GetRepartoRole(reparto, role);
                if (repartoRole != null)
                {
                    Program._log.Information("Éxito al obtener fila de RepartoRole con idReparto " + reparto + " e idRole " + role);
                    return Ok(repartoRole);
                }
                else
                {
                    Program._log.Warning("Error al obtener fila de RepartoRole con idReparto " + reparto + " e idRole " + role);
                    return NotFound();
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

        /// <summary>
        /// Inserción en RepartoRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /RepartoRole
        ///     {
        ///         "RepartoIdReparto": 0,
        ///         "RoleIdRole": 0
        ///     }
        /// </remarks>
        /// <param name="repRole"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al insertar en RepartoRole</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postrepartorole/")]
        public ActionResult PostRepartoRole([FromBody] ERepartoRole repRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RepartoRole repartoRole = new RepartoRole();
                    repartoRole.RepartoIdReparto = Convert.ToInt32(repRole.RepartoIdReparto);
                    repartoRole.RoleIdRole = Convert.ToInt32(repRole.RoleIdRole);

                    var repartoRoles = _repartoRole.PostRepartoRole(repartoRole);
                    if (repartoRoles != null)
                    {
                        Program._log.Information("Éxito al insertar fila en RepartoRole");
                        return Ok(repartoRoles);
                    }
                    else
                    {
                        Program._log.Warning("Error al insertar fila en RepartoRole");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al insertar fila en RepartoRole. Bad Request");
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

        /// <summary>
        /// Actualización de datos de RepartoRole
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /RepartoRole
        ///     {
        ///         "RepartoIdReparto": 0,
        ///         "RoleIdRole": 0
        ///     }
        /// </remarks>
        /// <param name="repRole"></param>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">RepartoRole Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putrepartorole/{reparto}/{role}")]
        public ActionResult PutRepartoRole([FromBody] ERepartoRole repRole, int reparto, int role)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    RepartoRole repartoRole = new RepartoRole();
                    repartoRole.RepartoIdReparto = Convert.ToInt32(repRole.RepartoIdReparto);
                    repartoRole.RoleIdRole = Convert.ToInt32(repRole.RoleIdRole);

                    answer = _repartoRole.PutRepartoRole(repartoRole, reparto, role);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de RepartoRole con idReparto " + reparto + " e idRole " + role);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de RepartoRole con idReparto " + reparto + " e idRole " + role);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos de RepartoRole. Bad Request");
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

        /// <summary>
        /// Borrado de RepartoROle
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">RepartoRole Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleterepartorole/{reparto}/{role}")]
        public ActionResult DeleteRepartoRole(int reparto, int role)
        {
            try
            {
                Boolean answer = _repartoRole.DeleteRepartoRole(reparto, role);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar RepartoRole con idReparto " + reparto + " e idRole " + role);
                    return Ok();
                }
                else
                {
                    Program._log.Warning("Error al borrar RepartoRole con idReparto " + reparto + " e idRole " + role);
                    return NotFound();
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
        #endregion

        #region SerieReparto 
        /// <summary>
        /// Obtención de todas las filas de SerieReparto
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getserierepartos/")]
        public ActionResult GetSerieRepartos()
        {
            try
            {
                return Ok(_serieReparto.GetAll());
            }
            catch (Exception ex)
            {
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                ObjectResult response = new ObjectResult("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                response.StatusCode = 500;
                return response;
            }
        }

        /// <summary>
        /// Obtención de una fila de SerieReparto
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieReparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpGet("getseriereparto/{serie}/{reparto}")]
        public ActionResult GetSerieReparto(int serie, int reparto)
        {
            try
            {
                var serieRepartos = _serieReparto.GetSerieReparto(serie, reparto);
                if (serieRepartos != null)
                {
                    Program._log.Information("Éxito al buscar SerieReparto con idSerie " + serie + " e idReparto " + reparto);
                    return Ok(serieRepartos);
                }
                else
                {
                    Program._log.Warning("Error al buscar SerieReparto con idSerie " + serie + " e idReparto " + reparto);
                    return NotFound();
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

        /// <summary>
        /// Inserción en SerieReparto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /SerieReparto
        ///     {
        ///         "SerieIdSerie": 0,
        ///         "RepartoIdReparto": 0
        ///     }
        /// </remarks>
        /// <param name="serRep"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">Error al insertar en SerieReparto</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPost("postseriereparto/")]
        public ActionResult PostSerieReparto([FromBody] ESerieReparto serRep)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SerieReparto serieReparto = new SerieReparto();
                    serieReparto.SerieIdSerie = Convert.ToInt32(serRep.SerieIdSerie);
                    serieReparto.RepartoIdReparto = Convert.ToInt32(serRep.RepartoIdReparto);

                    var serieRepartos = _serieReparto.PostSerieReparto(serieReparto);
                    if (serieRepartos != null)
                    {
                        Program._log.Information("Éxito al insertar en SerieReparto");
                        return Ok(serieRepartos);
                    }
                    else
                    {
                        Program._log.Warning("Error al insertar en SerieReparto");
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al insertar en SerieReparto. Bad Request");
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

        /// <summary>
        /// Actualización de datos en SerieReparto
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /SerieReparto
        ///     {
        ///         "SerieIdSerie": 0,
        ///         "RepartoIdReparto": 0
        ///     }
        /// </remarks>
        /// <param name="serRep"></param>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieReparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpPut("putseriereparto/{serie}/{reparto}")]
        public ActionResult PutSerieReparto([FromBody] ESerieReparto serRep, int serie, int reparto)
        {
            try
            {
                Boolean answer = false;
                if (ModelState.IsValid)
                {
                    SerieReparto serieReparto = new SerieReparto();
                    serieReparto.SerieIdSerie = Convert.ToInt32(serRep.SerieIdSerie);
                    serieReparto.RepartoIdReparto = Convert.ToInt32(serRep.RepartoIdReparto);

                    answer = _serieReparto.PutSerieReparto(serieReparto, serie, reparto);
                    if (answer)
                    {
                        Program._log.Information("Éxito al actualizar datos de SerieReparto con idSerie " + serie + " e idReparto " + reparto);
                        return Ok();
                    }
                    else
                    {
                        Program._log.Warning("Error al actualizar datos de SerieReparto con idSerie " + serie + " e idReparto " + reparto);
                        return NotFound();
                    }
                }
                else
                {
                    Program._log.Warning("Error al actualizar datos en SerieReparto. Bad Request");
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

        /// <summary>
        /// Borrado de SerieReparto
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request Error</response>        
        /// <response code="401">Authorization information is missing or invalid</response>
        /// <response code="403">Forbidden Access</response>
        /// <response code="404">SerieReparto Not Found</response>
        /// <response code="500">Internal Server Error</response> 
        [HttpDelete("deleteseriereparto/{serie}/{reparto}")]
        public ActionResult DeleteSerieReparto(int serie, int reparto)
        {
            try
            {
                Boolean answer = _serieReparto.DeleteSerieReparto(serie, reparto);
                if (answer)
                {
                    Program._log.Information("Éxito al borrar SerieReparto");
                    return Ok();
                }
                else
                {
                    Program._log.Warning("SerieReparto no encontrado");
                    return NotFound();
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
        #endregion
    }
}
