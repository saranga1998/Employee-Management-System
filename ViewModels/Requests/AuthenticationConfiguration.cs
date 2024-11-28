namespace EMS_Project.ViewModels.Requests
{
    public class AuthenticationConfiguration
    {
        //Secrect Keys
        public string AccessTokenSecret { get; set; }
        public string RefreshTokenSecret { get; set; }

        //Ports
        public string Issuer { get; set; }
        public string Audience { get; set; }

        //Expire Mins
        public int AccessTokenExpirationMinutes { get; set; }

        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
