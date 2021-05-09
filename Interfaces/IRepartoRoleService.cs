using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IRepartoRoleService
    {
        bool DeleteRepartoRole(int reparto, int role);
        ICollection<RepartoRole> GetAll();
        RepartoRole GetRepartoRole(int reparto, int role);
        RepartoRole PostRepartoRole(RepartoRole repartoRole);
        bool PutRepartoRole(RepartoRole repartoRole, int reparto, int role);
    }
}