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
        public InfoSerie GetInfoSerie(int serie)
        {
            Serie series = _serie.GetSerie(serie);

            InfoSerie info = new InfoSerie();
            info.IdSerie = series.IdSerie;
            info.Titulo = series.Titulo;
            info.Poster = series.Poster;
            info.Year = series.Year;
            info.Sinopsis = series.Sinopsis;
            info.Trailer = series.Trailer;
            info.Generos = _genero.GetGeneros(serie);
            info.LitaReparto = _reparto.GetRepartos(serie);

            return info;
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
