using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IRoleService
    {
        bool DeleteRole(int id);
        ICollection<Role> GetAll();
        Role GetRole(int id);
        Role PostRole(Role role);
        bool PutRole(Role role);
        Role Search(string cad);
    }
}