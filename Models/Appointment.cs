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

        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string SlotNo { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }

        // Foreign key property
        [ForeignKey("PatientId")]
        public Guid PatientId { get; set; }
    }
}
