namespace EMS_Project.ViewModels.Requests
{
    public class AuthenticatedUserResponse
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}
