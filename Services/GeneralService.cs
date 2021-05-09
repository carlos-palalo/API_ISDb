using API_ISDb.Interfaces;
using API_ISDb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneralService : IGeneralService
    {
        private proyectoContext _context;
        private ISerieService _serie;
        private IGeneroService _genero;
        private ISerieGeneroService _serieGenero;
        private ISerieRepartoService _serieReparto;
        private IRepartoService _reparto;
        private IRepartoRoleService _repartoRole;
        private IRoleService _role;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <param name="serieGenero"></param>
        /// <param name="serieReparto"></param>
        /// <param name="reparto"></param>
        /// <param name="repartoRole"></param>
        /// <param name="role"></param>
        public GeneralService(proyectoContext context, ISerieService serie, IGeneroService genero, ISerieGeneroService serieGenero, ISerieRepartoService serieReparto, IRepartoService reparto, IRepartoRoleService repartoRole, IRoleService role)
        {
            _context = context;
            _serie = serie;
            _genero = genero;
            _serieGenero = serieGenero;
            _serieReparto = serieReparto;
            _reparto = reparto;
            _repartoRole = repartoRole;
            _role = role;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Serie> GetAll()
        {
            return _context.Serie.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public string GetInfoSerie(int serie)
        {
            Serie series = _serie.GetSerie(serie);
            ICollection<Genero> generos = _genero.GetAll();
            ICollection<Reparto> reparto = _reparto.GetAll();
            ICollection<Role> role = _role.GetAll();

            List<string> lista = new List<string>();
            List<Director> director = new List<Director>();
            List<Escritor> escritor = new List<Escritor>();
            List<Actor> actor = new List<Actor>();

            foreach (Genero gen in generos)
            {
                SerieGenero sg = _serieGenero.GetSerieGenero(serie, gen.IdGenero);
                if (sg != null)
                {
                    lista.Add(gen.Nombre);
                }
            }

            foreach (Reparto rep in reparto)
            {
                SerieReparto sr = _serieReparto.GetSerieReparto(serie, rep.IdReparto);
                if (sr != null)
                {
                    foreach (Role rol in role)
                    {
                        RepartoRole rr = _repartoRole.GetRepartoRole(rep.IdReparto, rol.IdRole);
                        if (rr != null)
                        {
                            Console.WriteLine("aa");
                            switch (rol.Nombre)
                            {
                                case "director":
                                    director.Add(new Director(rep.Name, rol.Nombre));
                                    break;
                                case "writer":
                                    escritor.Add(new Escritor(rep.Name, rol.Nombre));
                                    break;
                                case "actor":
                                    actor.Add(new Actor(rep.Name, rep.Foto, rol.Nombre));
                                    break;
                            }
                        }
                    }
                }
            }

            InfoSerie infoSerie = new InfoSerie(series.IdSerie, series.Titulo, series.Poster, series.Year, series.Sinopsis, series.Trailer, lista, director, escritor, actor);
            string json = JsonConvert.SerializeObject(infoSerie);
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public ICollection<Serie> SearchSerie(string serie)
        {
            return _serie.SearchSerie(serie);
        }
    }
}
