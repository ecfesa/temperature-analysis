namespace temperature_analysis.Models
{
    public class ErrorViewModel
    {

        public string? Error;

        public ErrorViewModel()
        {
        }

        public ErrorViewModel(string error)
        {
            this.Error = error;
        }

        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}