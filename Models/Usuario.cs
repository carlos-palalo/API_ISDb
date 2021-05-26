using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
#nullable disable

namespace API_ISDb
{
    public partial class Usuario
    {
        public Usuario()
        {
            Review = new HashSet<Review>();
        }

        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Review> Review { get; set; }
    }
}
