namespace server.Model
{
    public class LoginRes
    {
        public LoginRes(User user,string token) {
            this.id = user.Id;
            this.token = token;
            ICollection<SpaceShipProfileRes> SpaceShipProfileRes = new List<SpaceShipProfileRes>();
            ICollection<MarsRoverProfileRes> MarsRoverProfileRes = new List<MarsRoverProfileRes>();

            foreach (var profile in user.SpaceShipProfiles)
            {
                SpaceShipProfileRes.Add(new SpaceShipProfileRes(profile));
            }

            foreach (var profile in user.MarsRoverProfiles)
            {
                MarsRoverProfileRes.Add(new MarsRoverProfileRes(profile));
            }

            this.spaceShipProfile = SpaceShipProfileRes;
            this.MarsRoverProfile = MarsRoverProfileRes;
        }
        public int id { get; set; }
        public string token { get; set; }
            
        public ICollection<SpaceShipProfileRes> spaceShipProfile { get; set; }
        public ICollection<MarsRoverProfileRes> MarsRoverProfile { get; set; }


    }
}
