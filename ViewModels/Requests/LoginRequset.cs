using System.ComponentModel.DataAnnotations;

namespace EMS_Project.ViewModels.Requests
{
    public class LoginRequset
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, ErrorMessage = "Username must be between 10 and 20 characters", MinimumLength = 10)]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(15, ErrorMessage = "Password must be between 6 and 100 characters", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
