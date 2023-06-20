namespace DocApp.Models
{
    public class TimeSlot
    {
        public string Name { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
