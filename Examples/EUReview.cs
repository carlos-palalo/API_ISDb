using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API_ISDb.Examples
{
    public class EUReview
    {
        public int IdReview { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int Puntuacion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioIdUsuario { get; set; }
        public int SerieIdSerie { get; set; }
    }
}