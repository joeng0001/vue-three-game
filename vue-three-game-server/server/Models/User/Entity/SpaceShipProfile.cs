using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Model
{
    [Table("SpaceShipProfiles")]
    public class SpaceShipProfile
    {
        [Key]
        public int Id { get; set; }

        public int ammo { get; set; } = 50;

        public int life { get; set; } = 50;

        public int energy { get; set; } = 60;

        public double energyConsume { get; set; } = 0.01;
        public double lifeConsume { get; set; } = 0.001;

        public int UserId { get; set; } // Foreign key for User

        [ForeignKey("UserId")]
        public User User { get; set; } // Navigation property

    }
}
