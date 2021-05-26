using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// RepartoService
    /// </summary>
    public class RepartoService : IRepartoService
    {
        private readonly proyectoContext _context;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        public RepartoService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo colección de Reparto 
        /// </summary>
        /// <returns></returns>
        public ICollection<Reparto> GetAll()
        {
            return _context.Reparto.ToArray();
        }

        /// <summary>
        /// Obtengo una fila de Reparto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Reparto GetReparto(int id)
        {
            return _context.Reparto.Find(id);
        }

        /// <summary>
        /// Obtengo todos los datos del Reparto de una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                ListaReparto lr = new ListaReparto
                {
                    IdReparto = item.IdReparto,
                    Name = item.Name,
                    Foto = item.Foto
                };
                lista.Add(lr.IdReparto, lr);
            }

            var r = reparto.Select(i => i.IdReparto)
                .ToList();

            var rr = _context.RepartoRole
                .Where(i => r.Contains(i.RepartoIdReparto))
                .ToArray();

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
        /// Añado un reparto si no existe ya
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
        /// Actualizo los datos de un reparto
        /// </summary>
        /// <param name="reparto"></param>
        /// <returns></returns>
        public Boolean PutReparto(Reparto reparto)
        {
            var v_reparto = _context.Reparto.SingleOrDefault(a => a.IdReparto == reparto.IdReparto);
            if (v_reparto != null)
            {
                // A la entrada que corresponde con v_reparto, le asigno los valores de reparto
                _context.Entry(v_reparto).CurrentValues.SetValues(reparto);
                _context.Update(v_reparto);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro un reparto
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
        /// Busco un reparto que coincida con la cad
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
