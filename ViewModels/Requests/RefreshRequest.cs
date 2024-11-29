using System.ComponentModel.DataAnnotations;

namespace EMS_Project.ViewModels.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; } 
    }
}
