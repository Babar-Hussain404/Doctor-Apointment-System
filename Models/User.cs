
namespace DocApp.Models
{
    public class User : BaseEntity
    {
        public string FName { get; set; } = string.Empty;

        public string LName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string DOB { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public string UserType { get; set; } = string.Empty;

        public string CNIC { get; set; } = string.Empty;

        public string Phoneno { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public bool TermsAndConditions { get; set; }
    }

}
