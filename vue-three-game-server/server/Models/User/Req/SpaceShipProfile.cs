namespace server.Model
{
    public class SpaceShipProfileReq
    {
        public int ammo { get; set; } = 50;

        public int life { get; set; } = 50;

        public int energy { get; set; } = 60;

        public double energyConsume { get; set; } = 0.01;
        public double lifeConsume { get; set; } = 0.001;
    }
}
