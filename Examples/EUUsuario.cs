using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API_ISDb.Examples
{
    public class EUUsuario
    {
        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }
    }
}