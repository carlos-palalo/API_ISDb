using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class SerieGenero
    {
        public int SerieIdSerie { get; set; }
        public int GeneroIdGenero { get; set; }

        public virtual Genero GeneroIdGeneroNavigation { get; set; }
        public virtual Serie SerieIdSerieNavigation { get; set; }
    }
}
