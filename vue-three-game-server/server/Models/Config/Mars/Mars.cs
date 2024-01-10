namespace server.Models.Config.Mars
{
    public class Mars
    {
        public Mars(int level)
        {
            this.oil = 10 - level;
            this.energy = 10 - level;
        }

        public int score { get; } = 0;
        public double oil { get; set; }

        public double energy { get; set; }
    }
}
