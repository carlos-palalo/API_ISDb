using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class Serie
    {
        public Serie()
        {
            Review = new HashSet<Review>();
            SerieGenero = new HashSet<SerieGenero>();
            SerieReparto = new HashSet<SerieReparto>();
        }

        public int IdSerie { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public int Year { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }

        public virtual ICollection<Review> Review { get; set; }
        public virtual ICollection<SerieGenero> SerieGenero { get; set; }
        public virtual ICollection<SerieReparto> SerieReparto { get; set; }
    }
}
