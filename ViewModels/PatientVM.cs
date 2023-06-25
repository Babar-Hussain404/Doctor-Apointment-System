using System.ComponentModel.DataAnnotations;

namespace DocApp.ViewModels
{
    public class PatientVM
    {
        [Required(ErrorMessage = "First name is required"), Display(Name = "First Name"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "First name should be between 5 and 50 characters")]
        public string FName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required"), Display(Name = "Last Name"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "Last name should be between 5 and 50 characters")]
        public string LName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required"),
            RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Phone Number must contain 11 digits.")]
        public string Phoneno { get; set; } = string.Empty;

        public string Disease { get; set; } = string.Empty;

        public string SlotNo { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
