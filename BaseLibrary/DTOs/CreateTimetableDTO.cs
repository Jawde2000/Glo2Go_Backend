using BaseLibrary.Models;

namespace BaseLibrary.DTOs
{
    public class CreateTimetableDTO 
    {
        public string? TimelineTitle { get; set; }
        public DateOnly? TimelineStartDate { get; set; }
        public DateOnly? TimelineEndDate { get; set; }
        public string? TravelerEmail { get; set; }
        public Traveler? Traveler { get; set; }
        public List<string>? CollaboratorEmails { get; set; }  // List of emails for collaborators
    }
}
