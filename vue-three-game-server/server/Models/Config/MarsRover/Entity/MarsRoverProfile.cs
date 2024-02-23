using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Model
{
    [Table("MarsRoverProfile")]
    public class MarsRoverProfile
    {

        [Key]
        public int Id { get; set; }

        public int oil { get; set; } = 10;

        public int energy { get; set; } = 10;

        public int UserId { get; set; } // Foreign key for User

        [ForeignKey("UserId")]
        public User User { get; set; } // Navigation property

    }
}
