using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class SerieReparto
    {
        public int SerieIdSerie { get; set; }
        public int RepartoIdReparto { get; set; }

        //private readonly Reparto _repartoIdRepartoNavigation;
        //public virtual Reparto RepartoIdRepartoNavigation => _repartoIdRepartoNavigation;

        //private readonly Reparto _serieIdSerieNavigation;
        //public virtual Reparto SerieIdSerieNavigation => _serieIdSerieNavigation;
        
        public virtual Reparto RepartoIdRepartoNavigation { get; set; }
        public virtual Serie SerieIdSerieNavigation { get; set; }
    }
}
