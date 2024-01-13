using System.ComponentModel.DataAnnotations;

namespace server.Model
{
    public class UserAuth
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
