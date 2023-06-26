using System.ComponentModel.DataAnnotations.Schema;

namespace DocApp.Models
{
    public class Appointment : BaseEntity
    {
        public enum StatusEnum
        {
            Pending,
            Active,
            Closed
        }

        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;

        public string SlotNo { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public int Age { get; set; }

        public string Disease { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Prescription { get; set; }

        // Foreign key property
        [ForeignKey("PatientId")]
        public Guid PatientId { get; set; }
    }
}
