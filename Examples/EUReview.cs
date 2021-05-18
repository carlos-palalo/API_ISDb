using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API_ISDb.Examples
{
    public class EUReview
    {
        public string IdReview { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Puntuacion { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioIdUsuario { get; set; }
        public string SerieIdSerie { get; set; }
    }
}