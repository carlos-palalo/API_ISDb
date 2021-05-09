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
    public class RepartoService : IRepartoService
    {
        private proyectoContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public RepartoService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Reparto> GetAll()
        {
            return _context.Reparto.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reparto GetReparto(int id)
        {
            return _context.Reparto.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public Reparto PostReparto(Reparto reparto)
        {
            Reparto rep = Search(reparto.Name);
            if (rep == null)
            {
                _context.Reparto.Add(reparto);
                _context.SaveChanges();
            }
            return Search(reparto.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public Boolean PutReparto(Reparto reparto)
        {
            var v_reparto = _context.Reparto.SingleOrDefault(a => a.IdReparto == reparto.IdReparto);
            if (v_reparto != null)
            {
                _context.Entry(v_reparto).CurrentValues.SetValues(reparto);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteReparto(int id)
        {
            var repartos = _context.Reparto.SingleOrDefault(a => a.IdReparto == id);
            if (repartos != null)
            {
                _context.Reparto.Remove(repartos);
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
        /// <param name="cad"></param>
        /// <returns></returns>
        public Reparto Search(string cad)
        {
            foreach (Reparto rep in _context.Reparto)
            {
                if (rep.Name.Equals(cad))
                {
                    return rep;
                }
            }
            return null;
        }
    }
}
