using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API_ISDb.Examples
{
    public class ERegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}