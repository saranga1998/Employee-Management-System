namespace EMS_Project.ViewModels.Requests
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }
}
