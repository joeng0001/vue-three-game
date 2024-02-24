using server.Model;

namespace server.Models.Config.Universe
{
    public class Universe
    {
        public Universe(SpaceShipProfileRes profile,int level = 1)
        {
            ammo = profile!=null?profile.ammo:50 + level * 5;
            life = profile != null ? profile.life : 50 + level * 5;
            energy = profile != null ? profile.energy : 60 - level * 5;
            energyConsume = profile != null ? profile.energyConsume : 0.01 * level;
            lifeConsume = profile != null ? profile.lifeConsume : 0.001 * level;

            maxLife = 50 + level * 5;
            maxAmmo = 50 + level * 5;
            maxEnergy = 60 - level * 5;
            maxScore = 20 + level * 2;
            maxMeteorNumber = 10 + level * 15;
            
            blackHole = new BlackHole(level);
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
        public double energyResume { get; } = 0.005;

        public double lifeConsume { get; }

        public SpaceShipSpeed speed { get; } = new SpaceShipSpeed();
        public BlackHole blackHole { get; }

    }
    public class SpaceShipSpeed
    {
        public double value { get; } = 0.15;
        public double factor { get; } = 1;
        public double rotateSpeed { get; } = 0.01;
        public double maxSideRotation { get; } = 0.2;
        public double minSideRotation { get; } = -0.2;
        public double maxFrontRotation { get; } = 0.2;
        public double minFrontRotation { get; } = 0.2;
        public double bulletSpeed { get; } = 0.3;
    }

    public class BlackHole
    {
        public BlackHole(int lv)
        {
            lifeConsume = 0.002 * lv;
        }
        public bool inBlackhole { get; } = false;
        public double lifeConsume { get; }
    }
}

