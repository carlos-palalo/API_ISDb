using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// RepartoRoleService
    /// </summary>
    public class RepartoRoleService : IRepartoRoleService
    {
        private readonly proyectoContext _context;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        public RepartoRoleService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo la colección de RepartoRole
        /// </summary>
        /// <returns></returns>
        public ICollection<RepartoRole> GetAll()
        {
            return _context.RepartoRole.ToArray();
        }

        /// <summary>
        /// Obtengo una fila de RepartoRole
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public RepartoRole GetRepartoRole(int reparto, int role)
        {
            return _context.RepartoRole.Find(reparto, role);
        }

        /// <summary>
        /// Añado una fila a RepartoRole si no existe ya
        /// </summary>
        /// <param name="repartoRole"></param>
        /// <returns></returns>
        public RepartoRole PostRepartoRole(RepartoRole repartoRole)
        {
            RepartoRole rr = GetRepartoRole(repartoRole.RepartoIdReparto, repartoRole.RoleIdRole);
            if (rr == null)
            {
                _context.RepartoRole.Add(repartoRole);
                _context.SaveChanges();
            }
            return GetRepartoRole(repartoRole.RepartoIdReparto, repartoRole.RoleIdRole);
        }

        /// <summary>
        /// Actualizo los datos de una fila de RepartoRole
        /// </summary>
        /// <param name="repartoRole"></param>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Boolean PutRepartoRole(RepartoRole repartoRole, int reparto, int role)
        {
            var v_repartoRole = _context.RepartoRole.SingleOrDefault(a => a.RepartoIdReparto == reparto && a.RoleIdRole == role);
            if (v_repartoRole != null)
            {
                //Primero borro la fila y luego la añado
                DeleteRepartoRole(reparto, role);
                PostRepartoRole(repartoRole);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro una fila de RepartoRole
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Boolean DeleteRepartoRole(int reparto, int role)
        {
            var repartoRoles = _context.RepartoRole.SingleOrDefault(a => a.RepartoIdReparto == reparto && a.RoleIdRole == role);
            if (repartoRoles != null)
            {
                _context.RepartoRole.Remove(repartoRoles);
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
