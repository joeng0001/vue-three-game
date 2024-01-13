using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace server.Models.Config.Earth
{
    public class Earth
    {
        private readonly int _level;
        public Earth(int level=1)
        {
            this._level = level;   
        }

        public int gravity { get;  } 

        public int friction { get;  }



    }

    
}