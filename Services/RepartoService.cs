using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public Reparto GetRepartoSerie(int id)
        {
            return _context.Reparto.Find(id);
        }

        public ICollection<ListaReparto> GetRepartos(int id)
        {
            /*
             Select idReparto, name, foto, role.nombre
	                FROM reparto as r, reparto_role as rr, role,  serie_reparto as sr
	                WHERE r.idReparto = rr.Reparto_idReparto
                    AND rr.Role_idRole = role.idRole
                    AND r.idReparto = sr.Reparto_idReparto
                    AND sr.Serie_idSerie = id;
             */
            IDictionary<int, ListaReparto> lista = new Dictionary<int, ListaReparto>();
            ICollection<ListaReparto> respuesta = new Collection<ListaReparto>();

            var sr = _context.SerieReparto
                .Where(i => i.SerieIdSerie == id)
                .Select(i => i.RepartoIdReparto)
                .ToList();

            var reparto = _context.Reparto
                .Where(item => sr.Contains(item.IdReparto));

            foreach (Reparto item in reparto.ToArray())
            {
                ListaReparto lr = new ListaReparto();
                lr.IdReparto = item.IdReparto;
                lr.Name = item.Name;
                lr.Foto = item.Foto;
                lista.Add(lr.IdReparto, lr);
            }

            var r = reparto.Select(i => i.IdReparto)
                .ToList();

            var rr = _context.RepartoRole
                .Where(i => r.Contains(i.RepartoIdReparto))
                .ToArray();
            //.Select(i => i.RoleIdRole)
            //.ToList();

            var role = _context.Role.ToArray();

            foreach (Role item in role)
            {
                var abc = rr.Where(i => i.RoleIdRole == item.IdRole);
                foreach (RepartoRole listaRR in abc)
                {
                    lista[listaRR.RepartoIdReparto].Role = item.Nombre;
                    respuesta.Add(lista[listaRR.RepartoIdReparto]);
                }
            }

            return respuesta;
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
