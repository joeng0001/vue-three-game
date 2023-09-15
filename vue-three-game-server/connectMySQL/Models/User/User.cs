using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace connectMySQL.Model
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
