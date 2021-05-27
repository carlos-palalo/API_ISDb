using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// SerieService
    /// </summary>
    public class SerieService : ISerieService
    {
        private readonly proyectoContext _context;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        public SerieService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo colección de series
        /// </summary>
        /// <returns></returns>
        public ICollection<Serie> GetAll()
        {
            return _context.Serie.ToArray();
        }

        /// <summary>
        /// Obtengo una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Serie GetSerie(int id)
        {
            return _context.Serie.Find(id);
        }

        /// <summary>
        /// Obtengo una lista con el id y el nombre de las series
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SearchSerie> SearchSerie()
        {
            IEnumerable<SearchSerie> series = new List<SearchSerie>();

            var v_serie = _context.Serie
                .ToArray()
                .Select(x => new SearchSerie()
                {
                    IdSerie = x.IdSerie,
                    Titulo = x.Titulo
                });

            if (v_serie != null)
            {
                series = v_serie;
            }

            return series;
        }

        /// <summary>
        /// Obtengo la serie que coincida con cad
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        public Serie Search(string cad)
        {
            var v_serie = _context.Serie.SingleOrDefault(a => a.Titulo.Equals(cad));
            if (v_serie != null)
            {
                return v_serie;
            }
            return v_serie;
        }

        /// <summary>
        /// Añado una serie
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public Serie PostSerie(Serie serie)
        {
            Serie ser = Search(serie.Titulo);
            if (ser == null)
            {
                _context.Serie.Add(serie);
                _context.SaveChanges();
            }
            return Search(serie.Titulo);
        }

        /// <summary>
        /// Actualizo una serie
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public Boolean PutSerie(Serie serie)
        {
            var v_serie = _context.Serie.SingleOrDefault(a => a.IdSerie == serie.IdSerie);
            if (v_serie != null)
            {
                _context.Entry(v_serie).CurrentValues.SetValues(serie);
                _context.Update(v_serie);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteSerie(int id)
        {
            var series = _context.Serie.SingleOrDefault(a => a.IdSerie == id);
            if (series != null)
            {
                _context.Serie.Remove(series);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
