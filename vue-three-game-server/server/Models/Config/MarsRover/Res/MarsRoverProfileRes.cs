namespace server.Model
{
    public class MarsRoverProfileRes
    {
        public MarsRoverProfileRes(MarsRoverProfile profile)
        {
            this.Id= profile.Id;
            this.oil = profile.oil;
            this.energy=profile.energy;

        }

        public int Id { get; set; }

        public int oil { get; set; } 

        public int energy { get; set; } 

    }
}
