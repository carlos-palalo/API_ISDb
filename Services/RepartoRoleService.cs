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
    public class RepartoRoleService : IRepartoRoleService
    {
        private proyectoContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public RepartoRoleService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<RepartoRole> GetAll()
        {
            return _context.RepartoRole.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reparto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public RepartoRole GetRepartoRole(int reparto, int role)
        {
            return _context.RepartoRole.Find(reparto, role);
        }

        /// <summary>
        /// 
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
        /// 
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
        /// 
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
