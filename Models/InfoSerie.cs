using System.Collections.Generic;

namespace API_ISDb.Models
{
    public class InfoSerie
    {
        public int IdSerie { get; set; }
        public string Titulo { get; set; }
        public string Poster { get; set; }
        public int Year { get; set; }
        public string Sinopsis { get; set; }
        public string Trailer { get; set; }
        public ICollection<Genero> Generos { get; set; }
        public ICollection<ListaReparto> ListaReparto { get; set; }
        public ICollection<ListaReview> ListaReview { get; set; }

        public InfoSerie() { }
    }
}
