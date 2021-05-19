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
    [Authorize(Roles="admin")]
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
        /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("generatebbdd/")]
        public ActionResult GenerateBBDD()
        {
            bool response = _imdb.GenerateBBDD();
            if (response)
                return Ok();
            else
                return NotFound();
            
        }
        #endregion

        #region Series 
        //GetSeries => getseries/
        /// <summary>
        /// GetSeries method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getseries/")]
        public ActionResult GetSeries()
        {
            return Ok(_serie.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getserie/{id}")]
        public ActionResult GetSerie(int id)
        {
            var series = _serie.GetSerie(id);
            if (series != null)
                return Ok(series);
            else
                return NotFound();

        }

        /// <summary>
        /// PostSerie method
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
        [HttpPost("postserie/")]
        public ActionResult PostSerie([FromBody] ESerie ser)
        {
            if (ModelState.IsValid)
            {
                Serie serie = new Serie();
                //serie.IdSerie = ser.IdSerie;
                serie.Titulo = ser.Titulo;
                serie.Poster = ser.Poster;
                serie.Year = Convert.ToInt32(ser.Year);
                serie.Sinopsis = ser.Sinopsis;
                serie.Trailer = ser.Trailer;

                var series = _serie.PostSerie(serie);
                return Ok(series);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putserie/")]
        public ActionResult PutSerie([FromBody] EUSerie ser)
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
        [HttpDelete("deleteserie/{id}")]
        public ActionResult DeleteSerie(int id)
        {
            Boolean answer = _serie.DeleteSerie(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region Usuario 
        //GetUsuarios => getusuarios/
        /// <summary>
        /// GetUsuarios method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusuarios/")]
        public ActionResult GetUsuarios()
        {
            return Ok(_usuario.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getusuario/{id}")]
        public ActionResult GetUsuario(int id)
        {
            var usuarios = _usuario.GetUsuario(id);
            if (usuarios != null)
                return Ok(usuarios);
            else
                return NotFound();

        }

        /// <summary>
        /// PostUsuario method
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
        [HttpPost("postusuario/")]
        public ActionResult PostUsuario([FromBody] EUsuario user)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                //usuario.IdUsuario = user.IdUsuario;
                usuario.Username = user.Username;
                usuario.Password = Encrypt.GetSHA256(user.Password);
                usuario.Email = user.Email;

                var usuarios = _usuario.PostUsuario(usuario);
                return Ok(usuarios);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putusuario/")]
        public ActionResult PutUsuario([FromBody] EUUsuario user)
        {
            Console.WriteLine(user.Email);
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.IdUsuario = user.IdUsuario;
                usuario.Username = user.Username;
                //usuario.Password = Encrypt.GetSHA256(user.Password);
                usuario.Email = user.Email;
                usuario.Tipo = user.Tipo;

                answer = _usuario.PutUsuario(usuario);
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
            Boolean answer = _usuario.DeleteUsuario(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region Review 
        //GetReview => getseries/
        /// <summary>
        /// GetReview method
        /// </summary>
        [HttpGet("getreviews/")]
        public ActionResult GetReviews()
        {
            return Ok(_review.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getreview/{id}")]
        public ActionResult GetReview(int id)
        {
            var reviews = _review.GetReview(id);
            if (reviews != null)
                return Ok(reviews);
            else
                return NotFound();

        }

        /// <summary>
        /// PostReview method
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
        [HttpPost("postreview/")]
        public ActionResult PostReview([FromBody] EReview rev)
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
                return Ok(reviews);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putreview/")]
        public ActionResult PutReview([FromBody] EUReview rev)
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
        [HttpDelete("deletereview/{id}")]
        public ActionResult DeleteReview(int id)
        {
            Boolean answer = _review.DeleteReview(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region Genero 
        //GetGeneros => getgeneros/
        /// <summary>
        /// GetGeneros method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getgeneros/")]
        public ActionResult GetGeneros()
        {
            return Ok(_genero.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getgenero/{id}")]
        public ActionResult GetGenero(int id)
        {
            var generos = _genero.GetGenero(id);
            if (generos != null)
                return Ok(generos);
            else
                return NotFound();

        }

        /// <summary>
        /// PostGenero method
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
        [HttpPost("postgenero/")]
        public ActionResult PostGenero([FromBody] EGenero gen)
        {
            if (ModelState.IsValid)
            {
                Genero genero = new Genero();
                //genero.IdGenero = gen.IdGenero;
                genero.Nombre = gen.Nombre;

                var generos = _genero.PostGenero(genero);
                return Ok(generos);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putgenero/")]
        public ActionResult PutGenero([FromBody] EUGenero gen)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                Genero genero = new Genero();
                genero.IdGenero = gen.IdGenero;
                genero.Nombre = gen.Nombre;

                answer = _genero.PutGenero(genero);
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
        [HttpDelete("deletegenero/{id}")]
        public ActionResult DeleteGenero(int id)
        {
            Boolean answer = _genero.DeleteGenero(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region SerieGenero 
        //GetSerieGenero => getseries/
        /// <summary>
        /// GetSerieGenero method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getseriegeneros/")]
        public ActionResult GetSerieGenero()
        {
            return Ok(_serieGenero.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        [HttpGet("getseriegenero/{serie}/{genero}")]
        public ActionResult GetSerieGenero(int serie, int genero)
        {
            var seriegeneros = _serieGenero.GetSerieGenero(serie, genero);
            if (seriegeneros != null)
                return Ok(seriegeneros);
            else
                return NotFound();

        }

        /// <summary>
        /// PostSerieGenero method
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
        [HttpPost("postseriegenero/")]
        public ActionResult PostSerieGenero([FromBody] ESerieGenero sergen)
        {
            if (ModelState.IsValid)
            {
                SerieGenero seriegenero = new SerieGenero();
                seriegenero.GeneroIdGenero = Convert.ToInt32(sergen.GeneroIdGenero);
                seriegenero.SerieIdSerie = Convert.ToInt32(sergen.SerieIdSerie);

                var serieGeneros = _serieGenero.PostSerieGenero(seriegenero);
                return Ok(serieGeneros);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putseriegenero/{serie}/{genero}")]
        public ActionResult PutSerieGenero([FromBody] ESerieGenero sergen, int serie, int genero)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                SerieGenero seriegenero = new SerieGenero();
                seriegenero.GeneroIdGenero = Convert.ToInt32(sergen.GeneroIdGenero);
                seriegenero.SerieIdSerie = Convert.ToInt32(sergen.SerieIdSerie);

                answer = _serieGenero.PutSerieGenero(seriegenero, serie, genero);
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
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        [HttpDelete("deleteseriegenero/{serie}/{genero}")]
        public ActionResult DeleteSerieGenero(int serie, int genero)
        {
            Boolean answer = _serieGenero.DeleteSerieGenero(serie, genero);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region Reparto
        /// //GetRepartos => getrepartos/
        /// <summary>
        /// GetRepartos method
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        [HttpGet("getrepartos/{serie}")]
        public ActionResult GetRepartos(int serie)
        {
            return Ok(_reparto.GetRepartos(serie));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getreparto/{id}")]
        public ActionResult GetReparto(int id)
        {
            var repartos = _reparto.GetReparto(id);
            if (repartos != null)
                return Ok(repartos);
            else
                return NotFound();

        }

        /// <summary>
        /// PostReparto method
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
        [HttpPost("postreparto/")]
        public ActionResult PostReparto([FromBody] EReparto rep)
        {
            if (ModelState.IsValid)
            {
                Reparto reparto = new Reparto();
                //reparto.IdReparto = rep.IdReparto;
                reparto.Name = rep.Name;
                reparto.Foto = rep.Foto;

                var repartos = _reparto.PostReparto(reparto);
                return Ok(repartos);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putreparto/")]
        public ActionResult PutReparto([FromBody] EUReparto rep)
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
        [HttpDelete("deletereparto/{id}")]
        public ActionResult DeleteReparto(int id)
        {
            Boolean answer = _reparto.DeleteReparto(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region Role 
        //GetRoles => getroles/
        /// <summary>
        /// GetRoles method
        /// </summary>
        [HttpGet("getroles/")]
        public ActionResult GetRoles()
        {
            return Ok(_role.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getrole/{id}")]
        public ActionResult GetRole(int id)
        {
            var roles = _role.GetRole(id);
            if (roles != null)
                return Ok(roles);
            else
                return NotFound();

        }

        /// <summary>
        /// PostRole method
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
        [HttpPost("postrole/")]
        public ActionResult PostRole([FromBody] ERole rol)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role();
                //role.IdRole = rol.IdRole;
                role.Nombre = rol.Nombre;

                var roles = _role.PostRole(role);
                return Ok(roles);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putrole/")]
        public ActionResult PutRole([FromBody] EURole rol)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                Role role = new Role();
                role.IdRole = rol.IdRole;
                role.Nombre = rol.Nombre;

                answer = _role.PutRole(role);
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
        [HttpDelete("deleterole/{id}")]
        public ActionResult DeleteRole(int id)
        {
            Boolean answer = _role.DeleteRole(id);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region RepartoRole 
        //GetRepartoRoles => getrepartorole/
        /// <summary>
        /// GetRepartoRoles method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getrepartoroles/")]
        public ActionResult GetRepartoRoles()
        {
            return Ok(_repartoRole.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("getrepartorole/{reparto}/{role}")]
        public ActionResult GetRepartoRole(int reparto, int role)
        {
            var repartoRole = _repartoRole.GetRepartoRole(reparto, role);
            if (repartoRole != null)
                return Ok(repartoRole);
            else
                return NotFound();

        }

        /// <summary>
        /// PostRepartoRole method
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
        [HttpPost("postrepartorole/")]
        public ActionResult PostRepartoRole([FromBody] ERepartoRole repRole)
        {
            if (ModelState.IsValid)
            {
                RepartoRole repartoRole = new RepartoRole();
                repartoRole.RepartoIdReparto = Convert.ToInt32(repRole.RepartoIdReparto);
                repartoRole.RoleIdRole = Convert.ToInt32(repRole.RoleIdRole);

                var repartoRoles = _repartoRole.PostRepartoRole(repartoRole);
                return Ok(repartoRoles);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putrepartorole/{reparto}/{role}")]
        public ActionResult PutRepartoRole([FromBody] ERepartoRole repRole, int reparto, int role)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                RepartoRole repartoRole = new RepartoRole();
                repartoRole.RepartoIdReparto = Convert.ToInt32(repRole.RepartoIdReparto);
                repartoRole.RoleIdRole = Convert.ToInt32(repRole.RoleIdRole);

                answer = _repartoRole.PutRepartoRole(repartoRole, reparto, role);
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
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpDelete("deleterepartorole/{reparto}/{role}")]
        public ActionResult DeleteRepartoRole(int reparto, int role)
        {
            Boolean answer = _repartoRole.DeleteRepartoRole(reparto, role);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion

        #region SerieReparto 
        //GetSerieRepartos => getserierepartos/
        /// <summary>
        /// GetSerieRepartos method
        /// </summary>
        /// <returns></returns>
        [HttpGet("getserierepartos/")]
        public ActionResult GetSerieRepartos()
        {
            return Ok(_serieReparto.GetAll());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        [HttpGet("getseriereparto/{serie}/{reparto}")]
        public ActionResult GetSerieReparto(int serie, int reparto)
        {
            var serieRepartos = _serieReparto.GetSerieReparto(serie, reparto);
            if (serieRepartos != null)
                return Ok(serieRepartos);
            else
                return NotFound();

        }

        /// <summary>
        /// PostSerieReparto method
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
        [HttpPost("postseriereparto/")]
        public ActionResult PostSerieReparto([FromBody] ESerieReparto serRep)
        {
            if (ModelState.IsValid)
            {
                SerieReparto serieReparto = new SerieReparto();
                serieReparto.SerieIdSerie = Convert.ToInt32(serRep.SerieIdSerie);
                serieReparto.RepartoIdReparto = Convert.ToInt32(serRep.RepartoIdReparto);

                var serieRepartos = _serieReparto.PostSerieReparto(serieReparto);
                return Ok(serieRepartos);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 
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
        [HttpPut("putseriereparto/{serie}/{reparto}")]
        public ActionResult PutSerieReparto([FromBody] ESerieReparto serRep, int serie, int reparto)
        {
            Boolean answer = false;
            if (ModelState.IsValid)
            {
                SerieReparto serieReparto = new SerieReparto();
                serieReparto.SerieIdSerie = Convert.ToInt32(serRep.SerieIdSerie);
                serieReparto.RepartoIdReparto = Convert.ToInt32(serRep.RepartoIdReparto);

                answer = _serieReparto.PutSerieReparto(serieReparto, serie, reparto);
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
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        [HttpDelete("deleteseriereparto/{serie}/{reparto}")]
        public ActionResult DeleteSerieReparto(int serie, int reparto)
        {
            Boolean answer = _serieReparto.DeleteSerieReparto(serie, reparto);
            if (answer)
                return Ok();
            else
                return NotFound();
        }
        #endregion
    }
}
