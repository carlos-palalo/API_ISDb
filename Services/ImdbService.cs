using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static API_ISDb.Models.RequestSeries;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ImdbService : IImdbService
    {
        private proyectoContext _context;
        private IConfiguration _config;
        private ISerieService _serie;
        private IGeneroService _genero;
        private IRepartoService _reparto;
        private IRepartoRoleService _repartoRole;
        private IRoleService _role;
        private ISerieGeneroService _serieGenero;
        private ISerieRepartoService _serieReparto;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="config"></param>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <param name="reparto"></param>
        /// <param name="repartoRole"></param>
        /// <param name="role"></param>
        /// <param name="serieGenero"></param>
        /// <param name="serieReparto"></param>
        public ImdbService(proyectoContext context, IConfiguration config, ISerieService serie, IGeneroService genero, IRepartoService reparto, IRepartoRoleService repartoRole, IRoleService role, ISerieGeneroService serieGenero, ISerieRepartoService serieReparto)
        {
            _context = context;
            _config = config;
            _serie = serie;
            _genero = genero;
            _reparto = reparto;
            _repartoRole = repartoRole;
            _role = role;
            _serieGenero = serieGenero;
            _serieReparto = serieReparto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Request(string url)
        {
            try
            {
                string response = "";
                using (var client = new WebClient())
                {
                    Console.WriteLine(url);
                    response = client.DownloadString(url);
                    if (!string.IsNullOrEmpty(response))
                    {
                        //Console.WriteLine(response);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Boolean GenerateBBDD()
        {
            string key = _config.GetSection("imdb").GetSection("api-key").Value;
            string urlBase = _config.GetSection("imdb").GetSection("urlBase").Value;
            string urlTop = "API/MostPopularTVs/" + key;
            string urlSerie = "en/API/Title/" + key + "/";
            string auxUrlSerie = "/FullCast%2CTrailer";
            string url = "";
            int cont = 0;   //MAX 100 PER DAY
            url = urlBase + urlTop;

            RequestSeries.Root responseTop250 = JsonConvert.DeserializeObject<RequestSeries.Root>(Request(url));

            foreach (Item serie in responseTop250.items)
            {
                if (cont >= 1)
                    return true;

                if (_serie.Search(serie.title) == null)
                {
                    url = urlBase + urlSerie + serie.id + auxUrlSerie;
                    RequestSerie.Root responseSerie = JsonConvert.DeserializeObject<RequestSerie.Root>(Request(url));
                    Console.WriteLine(responseSerie.title);

                    //Inserto la serie
                    var infoSerie = new Serie
                    {
                        Titulo = responseSerie.title,
                        Poster = responseSerie.image,
                        Year = Int32.Parse(responseSerie.year),
                        Trailer = responseSerie.trailer.linkEmbed,
                        Sinopsis = responseSerie.plot,
                        SerieGenero = new Collection<SerieGenero>
                        {
                            
                        }
                    };
                    _context.Add(infoSerie);
                    _context.SaveChanges();

                    //Obtengo id de la serie recien insertada
                    var idSerie = _context.Serie.FirstOrDefault(x => x.Titulo == responseSerie.title).IdSerie;

                    /******************************************************************************/

                    //Lista de nombres de generos en la DB
                    var listDbG = _context.Genero
                        .Select(x => x.Nombre)
                        .ToList();

                    //Lista de nombres de generos de la API externa
                    var listG = responseSerie.genreList
                        .Select(x => x.value)
                        .ToList();

                    //Compruebo si existen los generos de la consulta en la DB y selecciono sus ID
                    var gExist = _context.Genero
                        .Where(x => listG.Contains(x.Nombre))
                        .Select(x => x.IdGenero)
                        .ToList();

                    //Añado filas a SerieGenero en la DB con el idSerie y los ID de gExist
                    List<SerieGenero> sg = new List<SerieGenero>();
                    foreach (var id in gExist)
                    {
                        sg.Add(new SerieGenero { GeneroIdGenero = id, SerieIdSerie = idSerie });
                    }
                    _context.SerieGenero.AddRange(sg);

                    //Selecciono los generos que no están en la Db
                    var gNExist = responseSerie.genreList
                        .Where(x => !listDbG.Contains(x.value))
                        .Select(x => x.value)
                        .ToList();

                    //Añado los generos nuevos
                    List<Genero> g = new List<Genero>();
                    foreach (var name in gNExist)
                    {
                        g.Add(new Genero { Nombre = name });
                    }
                    _context.Genero.AddRange(g);

                    //Obtengo los ids de los generos recientemente añadidos
                    var idListG = _context.Genero
                        .Where(x => gNExist.Contains(x.Nombre))
                        .Select(x => x.IdGenero)
                        .ToList();

                    //Añado filas a SerieGenero con el idSerie y los ID de los generos nuevos
                    List<SerieGenero> sgnew = new List<SerieGenero>();
                    foreach (var id in idListG)
                    {
                        sgnew.Add(new SerieGenero { GeneroIdGenero = id, SerieIdSerie = idSerie });
                    }
                    _context.SerieGenero.AddRange(sgnew);

                    /******************************************************************************/

                    /******************************************************************************/

                    //Lista de nombres de la tabla Reparto
                    var listDbR = _context.Reparto.Select(x => x.Name).ToList();

                    //Lista de nombres de directores de la consulta
                    var listaRDir = responseSerie.fullCast.directors.items.Select(x => x.name);

                    //Lista de nombres de directores de la consulta
                    var listaRWrt = responseSerie.fullCast.writers.items.Select(x => x.name);

                    //Lista de nombres de directores de la consulta
                    var listaRAct = responseSerie.fullCast.actors.Select(x => x.name);

                    //Lista ID de directores en Db
                    var idRDirExist = _context.Reparto
                        .Where(x => listaRDir.Contains(x.Name))
                        .Select(x=>x.IdReparto).ToList();

                    //Lista ID de escritores en Db
                    var idRWrtExist = _context.Reparto
                        .Where(x => listaRWrt.Contains(x.Name))
                        .Select(x => x.IdReparto).ToList();

                    //Lista ID de actores en Db
                    var idRActExist = _context.Reparto
                        .Where(x => listaRAct.Contains(x.Name))
                        .Select(x => x.IdReparto).ToList();

                    /******************************************************************************/
                    cont++;

                };
            }

            /*
            foreach (Item item in responseTop250.items)
            {
                if (cont == 5)
                    return true;

                Serie ser = _serie.Search(item.title);
                if (ser == null)
                {
                    //Console.WriteLine(item.id);
                    string info = Request(url);
                    cont++;
                    //Console.WriteLine(info);
                    if (!info.Equals(""))
                    {
                        bool response = WriteBBDD(info);
                        if (!response)
                            return false;

                    }
                }
            }*/

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoserie"></param>
        /// <returns></returns>
        private Boolean WriteBBDD(string infoserie)
        {
            //Console.WriteLine(infoserie);
            RequestSerie.Root obj = JsonConvert.DeserializeObject<RequestSerie.Root>(infoserie);
            //Console.WriteLine(obj.id);

            Serie serie = new Serie();
            serie.Titulo = obj.title;
            serie.Poster = obj.image;
            if (obj.year == null)
                serie.Year = 0;
            else
                serie.Year = Int32.Parse(obj.year);
            serie.Sinopsis = obj.plot;
            //Console.WriteLine(obj.trailer);
            if (obj.trailer == null)
                return true;

            serie.Trailer = obj.trailer.linkEmbed;

            Serie ser = _serie.PostSerie(serie);

            WriteGenero(obj.genreList, ser);
            WriteItem(obj.fullCast.directors.items, ser, "director");
            WriteItem(obj.fullCast.writers.items, ser, "writer");
            WriteActor(obj.fullCast.actors, ser, "actor");

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        /// <param name="tipo"></param>
        private void WriteItem(List<RequestSerie.Item> items, Serie ser, string tipo)
        {
            foreach (RequestSerie.Item item in items)
            {
                Reparto reparto = new Reparto();
                reparto.Name = item.name;

                Reparto rep = _reparto.PostReparto(reparto);
                Role role = _role.Search(tipo);

                RepartoRole rr = new RepartoRole();
                rr.RepartoIdReparto = rep.IdReparto;
                rr.RoleIdRole = role.IdRole;
                //Console.WriteLine(rr.RepartoIdReparto + " - " + rr.RoleIdRole);
                _repartoRole.PostRepartoRole(rr);

                SerieReparto sr = new SerieReparto();
                sr.SerieIdSerie = ser.IdSerie;
                sr.RepartoIdReparto = rep.IdReparto;
                _serieReparto.PostSerieReparto(sr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        /// <param name="tipo"></param>
        private void WriteActor(List<RequestSerie.Actor> items, Serie ser, string tipo)
        {
            foreach (RequestSerie.Actor actor in items)
            {
                Reparto reparto = new Reparto();
                reparto.Name = actor.name;
                reparto.Foto = actor.image;
                Reparto rep = _reparto.PostReparto(reparto);

                Role role = _role.Search("actor");

                RepartoRole rr = new RepartoRole();
                rr.RepartoIdReparto = rep.IdReparto;
                rr.RoleIdRole = role.IdRole;
                _repartoRole.PostRepartoRole(rr);

                SerieReparto sr = new SerieReparto();
                sr.SerieIdSerie = ser.IdSerie;
                sr.RepartoIdReparto = rep.IdReparto;
                _serieReparto.PostSerieReparto(sr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        private void WriteGenero(List<RequestSerie.GenreList> items, Serie ser)
        {

            foreach (RequestSerie.GenreList genre in items)
            {
                Genero genero = new Genero();
                genero.Nombre = genre.value;
                Genero gen = _genero.PostGenero(genero);

                SerieGenero sg = new SerieGenero();
                sg.GeneroIdGenero = gen.IdGenero;
                sg.SerieIdSerie = ser.IdSerie;
                //Console.WriteLine(sg.GeneroIdGenero + " - " + sg.SerieIdSerie);
                _serieGenero.PostSerieGenero(sg);
            }
        }
    }
}
