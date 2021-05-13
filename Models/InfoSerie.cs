using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public ICollection<ListaReparto> LitaReparto { get; set; }

        public InfoSerie(int idSerie, string titulo, string poster, int year, string sinopsis, string trailer, ICollection<Genero> generos, ICollection<ListaReparto> litaReparto)
        {
            IdSerie = idSerie;
            Titulo = titulo;
            Poster = poster;
            Year = year;
            Sinopsis = sinopsis;
            Trailer = trailer;
            Generos = generos;
            LitaReparto = litaReparto;
        }

        public InfoSerie()
        {
        }
    }
}
