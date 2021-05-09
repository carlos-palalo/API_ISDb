using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SerieRepartoService : ISerieRepartoService
    {
        private proyectoContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SerieRepartoService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<SerieReparto> GetAll()
        {
            return _context.SerieReparto.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public SerieReparto GetSerieReparto(int serie, int reparto)
        {
            return _context.SerieReparto.Find(serie, reparto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serieReparto"></param>
        /// <returns></returns>
        public SerieReparto PostSerieReparto(SerieReparto serieReparto)
        {
            SerieReparto sr = GetSerieReparto(serieReparto.SerieIdSerie, serieReparto.RepartoIdReparto);
            if (sr == null)
            {
                _context.SerieReparto.Add(serieReparto);
                _context.SaveChanges();
            }
            return GetSerieReparto(serieReparto.SerieIdSerie, serieReparto.RepartoIdReparto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serieReparto"></param>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public Boolean PutSerieReparto(SerieReparto serieReparto, int serie, int reparto)
        {
            var v_serieReparto = _context.SerieReparto.SingleOrDefault(a => a.SerieIdSerie == serie && a.RepartoIdReparto == reparto);
            if (v_serieReparto != null)
            {
                DeleteSerieReparto(serie, reparto);
                PostSerieReparto(serieReparto);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public Boolean DeleteSerieReparto(int serie, int reparto)
        {
            var serieRepartos = _context.SerieReparto.SingleOrDefault(a => a.SerieIdSerie == serie && a.RepartoIdReparto == reparto);
            if (serieRepartos != null)
            {
                _context.SerieReparto.Remove(serieRepartos);
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
