using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class Reparto
    {
        public Reparto()
        {
            RepartoRole = new HashSet<RepartoRole>();
            SerieReparto = new HashSet<SerieReparto>();
        }

        public int IdReparto { get; set; }
        public string Name { get; set; }
        public string Foto { get; set; }

        public virtual ICollection<RepartoRole> RepartoRole { get; set; }
        public virtual ICollection<SerieReparto> SerieReparto { get; set; }
    }
}
