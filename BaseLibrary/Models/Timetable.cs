using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class Timetable
    {
        [Key]
        public string? TimelineID { get; set; }
        public string? TimelineTitle { get; set; }
        public DateOnly? TimelineStartDate { get; set; }
        public DateOnly? TimelineEndDate { get; set; }
        [ForeignKey("Traveler")]
        public string? TravelerEmail { get; set; }
        public Traveler? Traveler { get; set; }
        // Relationship to Traveler: One Timetable can have multiple collaborators
        public ICollection<TimetableCollaborator> Collaborators { get; set; } = new List<TimetableCollaborator>();
    }
}
