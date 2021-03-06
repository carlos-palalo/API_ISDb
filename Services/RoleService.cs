using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// RoleService
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly proyectoContext _context;

        /// <summary>
        /// Inyección dependencias
        /// </summary>
        /// <param name="context"></param>
        public RoleService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo colección de Role
        /// </summary>
        /// <returns></returns>
        public ICollection<Role> GetAll()
        {
            return _context.Role.ToArray();
        }

        /// <summary>
        /// Obtengo una fila de Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Role GetRole(int id)
        {
            return _context.Role.Find(id);
        }

        /// <summary>
        /// Añado una fila a Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role PostRole(Role role)
        {
            Role r = Search(role.Nombre);
            if (r == null)
            {
                _context.Role.Add(role);
                _context.SaveChanges();
            }
            return Search(role.Nombre);
        }

        /// <summary>
        /// Actualizo una fila de Role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Boolean PutRole(Role role)
        {
            var v_role = _context.Role.SingleOrDefault(a => a.IdRole == role.IdRole);
            if (v_role != null)
            {
                _context.Entry(v_role).CurrentValues.SetValues(role);
                _context.Update(v_role);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Borro una fila de Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteRole(int id)
        {
            var roles = _context.Role.SingleOrDefault(a => a.IdRole == id);
            if (roles != null)
            {
                _context.Role.Remove(roles);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Busco un Role que corresponda con la cad
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        public Role Search(string cad)
        {
            var v_role = _context.Role.SingleOrDefault(a => a.Nombre.Contains(cad));
            if (v_role != null)
            {
                return v_role;
            }
            return v_role;
        }
    }
}
