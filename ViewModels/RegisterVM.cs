using System.ComponentModel.DataAnnotations;

namespace DocApp.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First name is required"), Display(Name = "First Name"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "First name should be between 5 and 50 characters")]
        public string FName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required"), Display(Name = "Last Name"),
            StringLength(50, MinimumLength = 3, ErrorMessage = "Last name should be between 5 and 50 characters")]
        public string LName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        //At least one letter((?=.*[A - Za - z]))
        //At least one digit((?=.*\\d))
        //A length between 5 and 50 characters([A-Za-z\\d]{ 5,50})
        [Required(ErrorMessage = "Password is required"), DataType(DataType.Password),
            RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{5,50}$")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required"), DataType(DataType.Date), Display(Name = "Date of Birth")]
        public string DOB { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required"),
            RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Type is required"),
            RegularExpression("^(Staff|Doctor|Patient)$", ErrorMessage = "Invalid User Type"), Display(Name = "User Type")]
        public string UserType { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNIC is required"),
            RegularExpression("^[0-9]{5}-[0-9]{7}-[0-9]$", ErrorMessage = "Invalid CNIC number")]
        public string CNIC { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required"),
            RegularExpression(@"^([0-9]{11})$", ErrorMessage = "Phone Number must contain 11 digits.")]
        public string Phoneno { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;


        [Required(ErrorMessage = "You must agree to the terms and conditions.")]
        public bool TermsAndConditions { get; set; }

    }

}
