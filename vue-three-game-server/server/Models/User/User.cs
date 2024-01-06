using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace server.Model
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public byte[] password { get; set; }

        public byte[] salt { get; set; }
    }

    public class UserAuth
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
