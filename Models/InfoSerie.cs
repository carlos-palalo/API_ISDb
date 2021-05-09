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

        public List<string> generos { get; set; }

        public List<Director> director { get; set; }
        public List<Escritor> escritor { get; set; }
        public List<Actor> actor { get; set; }

        public InfoSerie(int idSerie, string titulo, string poster, int year, string sinopsis, string trailer, List<string> generos, List<Director> director, List<Escritor> escritor, List<Actor> actor)
        {
            IdSerie = idSerie;
            Titulo = titulo;
            Poster = poster;
            Year = year;
            Sinopsis = sinopsis;
            Trailer = trailer;
            this.generos = generos;
            this.director = director;
            this.escritor = escritor;
            this.actor = actor;
        }
    }

    public class Director
    {
        public string nombre { get; set; }
        public string ocupacion { get; set; }

        public Director(string nombre, string ocupacion)
        {
            this.nombre = nombre;
            this.ocupacion = ocupacion;
        }
    }

    public class Escritor
    {
        public string nombre { get; set; }
        public string ocupacion { get; set; }

        public Escritor(string nombre, string ocupacion)
        {
            this.nombre = nombre;
            this.ocupacion = ocupacion;
        }
    }

    public class Actor
    {
        public string nombre { get; set; }
        public string foto { get; set; }
        public string ocupacion { get; set; }

        public Actor(string nombre, string foto, string ocupacion)
        {
            this.nombre = nombre;
            this.foto = foto;
            this.ocupacion = ocupacion;
        }
    }
}
