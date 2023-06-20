namespace DocApp.Models
{
    public class Patient : User
    {
        public string Disease { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
    }
}
