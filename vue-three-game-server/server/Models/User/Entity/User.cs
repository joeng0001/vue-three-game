using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace server.Model
{
    [Table("Users")]
    public class User
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;

        public byte[] password { get; set; }

        public byte[] salt { get; set; }
    }
}
