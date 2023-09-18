using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace connectMySQL.Model
{
    public class Earth
    {

        public int gravity { get; set; }

        public int friction { get; set; }


        
    }
}