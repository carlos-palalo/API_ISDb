using API_ISDb.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_ISDb
{
    public partial class SearchSerie
    { 
        public int IdSerie { get; set; }
        public string Titulo { get; set; }
    }
}
