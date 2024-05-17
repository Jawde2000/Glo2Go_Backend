using BaseLibrary.DTOs;

public class TimetableDTO
{
    public string? TimelineID { get; set; }
    public string? TimelineTitle { get; set; }
    public DateOnly? TimelineStartDate { get; set; }
    public DateOnly? TimelineEndDate { get; set; }
    public List<string>? CollaboratorEmails { get; set; }  // List of emails for collaborators
}