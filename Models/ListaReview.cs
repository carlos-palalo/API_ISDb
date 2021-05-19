using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class ListaReview
    {
        public string Usuario { get; set; }
        public string Title { get; set; }
        public string Descripcion { get; set; }
        public int Puntuacion { get; set; }
        public string Fecha { get; set; }

        public ListaReview(string usuario, string title, string descripcion, int puntuacion, string fecha)
        {
            Usuario = usuario;
            Title = title;
            Descripcion = descripcion;
            Puntuacion = puntuacion;
            Fecha = fecha;
        }
    }
}
