namespace EMS_Project.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public IEnumerable<string>? ErrorMessages { get; set; }

        public ErrorViewModel(string errorMessage)
        {
            List<string> list = new List<string> { "message", errorMessage };

        }


        public ErrorViewModel(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}
