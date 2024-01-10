namespace server.Models.Config.Mars
{
    public class Mars
    {
        public Mars(int level=1)
        {
            this.oil = 10 - level;
            this.energy = 10 - level;
        }

        public int score { get; } = 0;
        public double oil { get; set; }

        public double energy { get; set; }

        public Object gravity { get;  } = new { x = 0, y = -9.82, z = 0 };

        public Object bodyGroundContact { get; } = new { friction = 0.1, restitution = 0.3 };

        public List<List<double>> preDefineColors { get; } = new List<List<double>>
        {
            new List<double> { 0.764705882352941, 0.3137254901960784, 0.1411764705882353 },
            new List<double> { 0.8274509803921568, 0.4117647058823529, 0.19607843137254902 },
        };

    }
}
