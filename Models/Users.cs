using System.ComponentModel.DataAnnotations;

namespace API_ISDb.Models
{
    public class Users
    {
        public Users(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public Users() { }

        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
