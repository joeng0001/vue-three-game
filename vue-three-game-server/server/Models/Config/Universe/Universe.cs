namespace server.Models.Config.Universe
{
    public class Universe
    {

        public Universe(int level=1)
        {
            this.ammo = 50 + level * 5;
            this.life = 50 + level * 5;
            this.energy = 60 - level * 5;
            this.maxLife = 50 + level * 5;
            this.maxAmmo = 50 + level * 5;
            this.maxEnergy = 60 - level * 5;
            this.maxScore = 20 + level * 2;
            this.maxMeteorNumber = 10 + level * 15;
            this.energyConsume = 0.01 * level;
            this.lifeConsume = 0.001 * level;
        }

        public double ColiisionDistance { get; } = 0;
        public int score { get; } = 0;

        public int ammo { get; }
        public int life { get; }
        public int energy { get; }
        public int maxLife { get; }
        public int maxAmmo { get; }
        public int maxEnergy { get; }
        public int maxScore { get; }
        public int maxMeteorNumber { get; }
        public double energyConsume { get; }
        public double energyResume {get;}=0.005;

        public double lifeConsume { get; }

        public SpaceShipSpeed SpaceShipSpeed { get; } = new SpaceShipSpeed();

    }
    public class SpaceShipSpeed
    {
        public double value{get;} = 0.15;
        public double factor {get;}= 1;

        public double rotateSpeed { get; } = 0.01;
        public double maxSideRotation {get;}= 0.2;
        public double minSideRotation {get;}= -0.2;
        public double maxFrontRotation{get;} = 0.2;
        public double minFrontRotation {get;}= 0.2;
        public double bulletSpeed {get;}= 0.3;
    }
}

