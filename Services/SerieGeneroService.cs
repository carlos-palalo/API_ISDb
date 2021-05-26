using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// SerieGeneroService
    /// </summary>
    public class SerieGeneroService : ISerieGeneroService
    {
        private readonly proyectoContext _context;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        public SerieGeneroService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo colección de SerieGenero
        /// </summary>
        /// <returns></returns>
        public ICollection<SerieGenero> GetAll()
        {
            return _context.SerieGenero.ToArray();
        }

        /// <summary>
        /// Obtengo una fila de SerieGenero
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public SerieGenero GetSerieGenero(int serie, int genero)
        {
            return _context.SerieGenero.Find(serie, genero);
        }

        /// <summary>
        /// Añado una fila a SerieGenero
        /// </summary>
        /// <param name="serieGenero"></param>
        /// <returns></returns>
        public SerieGenero PostSerieGenero(SerieGenero serieGenero)
        {
            SerieGenero sg = GetSerieGenero(serieGenero.SerieIdSerie, serieGenero.GeneroIdGenero);
            if (sg == null)
            {
                _context.SerieGenero.Add(serieGenero);
                _context.SaveChanges();
            }
            return GetSerieGenero(serieGenero.SerieIdSerie, serieGenero.GeneroIdGenero);
        }

        /// <summary>
        /// Actualizo una fila de SerieGenero
        /// </summary>
        /// <param name="serieGenero"></param>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public Boolean PutSerieGenero(SerieGenero serieGenero, int serie, int genero)
        {
            var v_serieGenero = _context.SerieGenero.SingleOrDefault(a => a.SerieIdSerie == serie && a.GeneroIdGenero == genero);
            if (v_serieGenero != null)
            {
                DeleteSerieGenero(serie, genero);
                PostSerieGenero(serieGenero);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro una fila de SerieGenero
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="genero"></param>
        /// <returns></returns>
        public Boolean DeleteSerieGenero(int serie, int genero)
        {
            var serieGeneros = _context.SerieGenero.SingleOrDefault(a => a.SerieIdSerie == serie && a.GeneroIdGenero == genero);
            if (serieGeneros != null)
            {
                _context.SerieGenero.Remove(serieGeneros);
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
