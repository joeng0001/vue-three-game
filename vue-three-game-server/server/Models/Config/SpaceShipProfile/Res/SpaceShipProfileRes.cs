namespace server.Model
{
    public class SpaceShipProfileRes
    {
        public SpaceShipProfileRes(SpaceShipProfile profile)
        {
            this.Id= profile.Id;
            this.ammo = profile.ammo;
            this.life=profile.life;
            this.energy=profile.energy;
            this.energyConsume=profile.energyConsume;
            this.lifeConsume=profile.lifeConsume;
        }

        public int Id { get; set; }
        public int ammo { get; set; }

        public int life { get; set; } 

        public int energy { get; set; } 

        public double energyConsume { get; set; } 
        public double lifeConsume { get; set; } 
    }
}
