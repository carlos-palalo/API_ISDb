using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class ListaReparto
    {
        public int IdReparto { get; set; }
        public string Name { get; set; }
        public string Foto { get; set; }
        public string Role { get; set; }

        public ListaReparto(int idReparto, string name, string foto, string role)
        {
            IdReparto = idReparto;
            Name = name;
            Foto = foto;
            Role = role;
        }

        public ListaReparto() { }
    }
}
