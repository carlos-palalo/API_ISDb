using API_ISDb.Interfaces;
using API_ISDb.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using static API_ISDb.Models.RequestSeries;

namespace API_ISDb.Services
{
    /// <summary>
    ///Consultar API IMDb e insertar datos obtenidos
    /// </summary>
    public class ImdbService : IImdbService
    {
        private readonly IConfiguration _config;
        private readonly ISerieService _serie;
        private readonly IGeneroService _genero;
        private readonly IRepartoService _reparto;
        private readonly IRepartoRoleService _repartoRole;
        private readonly IRoleService _role;
        private readonly ISerieGeneroService _serieGenero;
        private readonly ISerieRepartoService _serieReparto;

        public ImdbService(IConfiguration config, ISerieService serie, IGeneroService genero, IRepartoService reparto, IRepartoRoleService repartoRole, IRoleService role, ISerieGeneroService serieGenero, ISerieRepartoService serieReparto)
        {
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
        /// HTTP Request
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
                Program._log.Fatal("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                return "";
            }
        }

        /// <summary>
        /// Inserta en las tablas
        /// </summary>
        /// <returns></returns>
        public Boolean GenerateBBDD()
        {
            string key = _config.GetSection("imdb").GetSection("api-key").Value;
            string urlBase = _config.GetSection("imdb").GetSection("urlBase").Value;
            string urlTop = "API/MostPopularTVs/" + key;
            string urlSerie = "en/API/Title/" + key + "/";
            string auxUrlSerie = "/FullCast%2CTrailer";
            int cont = 0;   //El número máximo de peticiones por día es 100
            string url = urlBase + urlTop;

            //Obtengo resultado de la petición y lo convierto a JSON
            RequestSeries.Root responseTop250 = JsonConvert.DeserializeObject<RequestSeries.Root>(Request(url));

            //Recorro las series del Top250
            foreach (Item item in responseTop250.items)
            {
                if (cont == 3)
                    return true;

                //Si la serie no existe en la Db, la añado con todos sus datos
                Serie ser = _serie.Search(item.title);
                if (ser == null)
                {
                    //Si la petición me devuelve algo, escribo en la Db
                    url = urlBase + urlSerie + item.id + auxUrlSerie;
                    string info = Request(url);
                    if (!info.Equals(""))
                    {
                        bool response = WriteBBDD(info);
                        if (!response)
                            return false;
                    }
                    cont++;
                }
            }

            return true;
        }

        /// <summary>
        /// Escribe en BBDD los datos obtenidos
        /// </summary>
        /// <param name="infoserie"></param>
        /// <returns></returns>
        private Boolean WriteBBDD(string infoserie)
        {
            try
            {
                //Hago la petición y la convierto a JSON
                RequestSerie.Root obj = JsonConvert.DeserializeObject<RequestSerie.Root>(infoserie);

                //  ?? => si el objeto de la izquierda es null, pongo el valor de la derecha a la variable, sino, el de la izquierda
                Serie serie = new Serie
                {
                    Titulo = obj.title,
                    Poster = obj.image ?? "null",
                    Year = obj.year == null ? 0 : Int32.Parse(obj.year),
                    Sinopsis = obj.plot ?? "null",
                    Trailer = obj.trailer.linkEmbed ?? "null"
                };

                //Añado la serie o recupero si ya existiera
                Serie ser = _serie.PostSerie(serie);

                //Añado los generos, directores, escritores y actores a la Db
                WriteGenero(obj.genreList, ser);
                WriteItem(obj.fullCast.directors.items, ser, "director");
                WriteItem(obj.fullCast.writers.items, ser, "writer");
                WriteActor(obj.fullCast.actors, ser, "actor");

                return true;
            }
            catch(Exception ex)
            {
                Program._log.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Inserta directores y escritores
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        /// <param name="tipo"></param>
        private void WriteItem(List<RequestSerie.Item> items, Serie ser, string tipo)
        {
            foreach (RequestSerie.Item item in items)
            {
                Reparto reparto = new Reparto
                {
                    Name = item.name
                };

                //Inserto Reparto en la Bd y recupero el añadido, o si ya existiera
                Reparto rep = _reparto.PostReparto(reparto);
                //Busco el role [tipo] => director o escritor
                Role role = _role.Search(tipo);

                RepartoRole rr = new RepartoRole
                {
                    RepartoIdReparto = rep.IdReparto,
                    RoleIdRole = role.IdRole
                };
                _repartoRole.PostRepartoRole(rr);

                SerieReparto sr = new SerieReparto
                {
                    SerieIdSerie = ser.IdSerie,
                    RepartoIdReparto = rep.IdReparto
                };
                _serieReparto.PostSerieReparto(sr);
            }
        }

        /// <summary>
        /// Inserta los actores
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        /// <param name="tipo"></param>
        private void WriteActor(List<RequestSerie.Actor> items, Serie ser, string tipo)
        {
            foreach (RequestSerie.Actor actor in items)
            {
                Reparto reparto = new Reparto
                {
                    Name = actor.name,
                    Foto = actor.image
                };
                Reparto rep = _reparto.PostReparto(reparto);

                Role role = _role.Search(tipo);

                RepartoRole rr = new RepartoRole
                {
                    RepartoIdReparto = rep.IdReparto,
                    RoleIdRole = role.IdRole
                };
                _repartoRole.PostRepartoRole(rr);

                SerieReparto sr = new SerieReparto
                {
                    SerieIdSerie = ser.IdSerie,
                    RepartoIdReparto = rep.IdReparto
                };
                _serieReparto.PostSerieReparto(sr);
            }
        }

        /// <summary>
        /// Inserta los generos
        /// </summary>
        /// <param name="items"></param>
        /// <param name="ser"></param>
        private void WriteGenero(List<RequestSerie.GenreList> items, Serie ser)
        {
            foreach (RequestSerie.GenreList genre in items)
            {
                Genero genero = new Genero
                {
                    Nombre = genre.value
                };
                Genero gen = _genero.PostGenero(genero);

                SerieGenero sg = new SerieGenero
                {
                    GeneroIdGenero = gen.IdGenero,
                    SerieIdSerie = ser.IdSerie
                };
                _serieGenero.PostSerieGenero(sg);
            }
        }
    }
}
