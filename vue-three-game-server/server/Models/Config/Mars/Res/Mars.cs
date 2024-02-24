using server.Model;

namespace server.Models.Config.Mars
{
    public class Mars
    {
        public Mars(MarsRoverProfileRes profile,int level=1)
        {
            this.maxScore = 5+ 2 * level;
            this.numOfRock = 20 + level * 3;

            this.oil = this.maxOil = profile != null ? profile.oil : 10-level;
            this.energy=this.maxEnergy= profile != null ? profile.energy : 10 - level;
          
        }

        public int score { get; } = 0;
        public int maxScore { get;  }
        public double oil { get;  }
        public double maxOil { get;  }
        public double energy { get;  }
        public double maxEnergy { get;  }

        public int numOfRock { get; }

        public Object gravity { get;  } = new { x = 0, y = -9.82, z = 0 };

        public Object bodyGroundContact { get; } = new { friction = 0.1, restitution = 0.3 };

        public List<List<double>> preDefineColors { get; } = new List<List<double>>
        {
            new List<double> { 0.764705882352941, 0.3137254901960784, 0.1411764705882353 },
            new List<double> { 0.8274509803921568, 0.4117647058823529, 0.19607843137254902 },
        };

    }
}
