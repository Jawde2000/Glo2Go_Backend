namespace BaseLibrary.DTOs
{
    public class CreateActivity
    {
        public string? ActivityTitle { get; set; }
        public DateTime? ActivityStartTime { get; set; }
        public DateTime? ActivityEndTime { get; set; }
        public string? ActivityType { get; set; }
        public string? ActivityDescription { get; set; }
    }
}
