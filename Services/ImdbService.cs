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
            Reparto addRep;
            List<string> list = new List<string>();
            List<string> role;

            RequestSeries.Root responseTop250 = JsonConvert.DeserializeObject<RequestSeries.Root>(Request(url));

            foreach (Item item in responseTop250.items)
            {
                if (cont == 3)
                    return true;

                Serie ser = _serie.Search(item.title);
                if (ser == null)
                {
                    url = urlBase + urlSerie + item.id + auxUrlSerie;
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
            }

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

            serie.Poster = obj.image == null ? "null" : obj.image;
            serie.Year = obj.year == null ? 0 : Int32.Parse(obj.year);
            serie.Sinopsis = obj.plot == null ? "null" : obj.plot;
            serie.Trailer = obj.trailer.linkEmbed == null ? "null" : obj.trailer.linkEmbed;

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

                Role role = _role.Search(tipo);

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
