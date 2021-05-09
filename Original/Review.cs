using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class Review
    {
        public int IdReview { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Puntuacion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioIdUsuario { get; set; }
        public int SerieIdSerie { get; set; }

        public virtual Serie SerieIdSerieNavigation { get; set; }
        public virtual Usuario UsuarioIdUsuarioNavigation { get; set; }
    }
}
