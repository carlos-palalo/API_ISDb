using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Models
{
    public class Serie
    {
        [Key]
        public int idSerie { get; set; }
        public string titulo { get; set; }
        public string poster { get; set; }
        public int year { get; set; }
        public string sinopsis { get; set; }
        public string trailer { get; set; }
    }
}
