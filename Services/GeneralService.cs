using API_ISDb.Interfaces;
using API_ISDb.Models;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// GeneralService
    /// </summary>
    public class GeneralService : IGeneralService
    {
        private readonly proyectoContext _context;
        private readonly ISerieService _serie;
        private readonly IGeneroService _genero;
        private readonly IRepartoService _reparto;
        private readonly IReviewService _review;

        public GeneralService(proyectoContext context, ISerieService serie, IGeneroService genero, IRepartoService reparto, IReviewService review)
        {
            _context = context;
            _serie = serie;
            _genero = genero;
            _reparto = reparto;
            _review = review;
        }

        /// <summary>
        /// Obtengo una colección de Series
        /// </summary>
        /// <returns></returns>
        public ICollection<Serie> GetAll()
        {
            return _context.Serie.ToArray();
        }

        /// <summary>
        /// Obtengo toda la información disponible de una serie
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public InfoSerie GetInfoSerie(int serie)
        {
            Serie series = _serie.GetSerie(serie);

            InfoSerie info = new InfoSerie
            {
                IdSerie = series.IdSerie,
                Titulo = series.Titulo,
                Poster = series.Poster,
                Year = series.Year,
                Sinopsis = series.Sinopsis,
                Trailer = series.Trailer,
                Generos = _genero.GetGeneros(serie),
                ListaReparto = _reparto.GetRepartos(serie),
                ListaReview = _review.GetListaReviews(serie)
            };

            return info;
        }

        /// <summary>
        /// Obtengo el id y nombre de todas las series
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SearchSerie> SearchSerie()
        {
            return _serie.SearchSerie();
        }
    }
}
