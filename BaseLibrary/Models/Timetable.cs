using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class Timetable
    {
        [Key]
        public string? TimelineID { get; set; }
        public string? TimelineTitle { get; set; }
        public string? Country { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? TimelineStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? TimelineEndDate { get; set; }
        public string? Region { get; set; }

        [ForeignKey("Traveler")]
        public string? TravelerEmail { get; set; }
        // Relationship to Traveler: One Timetable can have multiple collaborators
        public ICollection<TimetableCollaborator> Collaborators { get; set; } = new List<TimetableCollaborator>();
        // New relationship: One Timetable can have multiple TimetableRegions
        public ICollection<TimetableRegions> Regions { get; set; } = new List<TimetableRegions>();
    }
}
