using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class Role
    {
        public Role()
        {
            RepartoRole = new HashSet<RepartoRole>();
        }

        public int IdRole { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<RepartoRole> RepartoRole { get; set; }
    }
}
