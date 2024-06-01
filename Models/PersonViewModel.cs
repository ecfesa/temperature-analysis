namespace temperature_analysis.Models
{
    public class PersonViewModel : StandardViewModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public int ThemeId { get; set; }

        public string ThemeDescription { get; set; }
        public string ThemeHex {  get; set; }
    }
}