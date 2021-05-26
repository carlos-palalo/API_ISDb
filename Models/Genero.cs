using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class Genero
    {
        public Genero()
        {
            SerieGenero = new HashSet<SerieGenero>();
        }

        public int IdGenero { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<SerieGenero> SerieGenero { get; set; }
    }
}
