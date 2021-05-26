// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class RepartoRole
    {
        public int RepartoIdReparto { get; set; }
        public int RoleIdRole { get; set; }

        public virtual Reparto RepartoIdRepartoNavigation { get; set; }
        public virtual Role RoleIdRoleNavigation { get; set; }

        //private readonly Reparto _repartoIdRepartoNavigation;
        //public virtual Reparto RepartoIdRepartoNavigation => _repartoIdRepartoNavigation;

        //private readonly Role _roleIdRoleNavigation;
        //public virtual Role RoleIdRoleNavigation => _roleIdRoleNavigation;
    }
}
