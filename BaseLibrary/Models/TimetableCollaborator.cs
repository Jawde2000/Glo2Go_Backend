using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class TimetableCollaborator
    {
        [Key]
        public string? Id { get; set; }  // Primary key

        [ForeignKey("Timetable")]
        public string? TimelineID { get; set; }
        public Timetable? Timetable { get; set; }

        [ForeignKey("Traveler")]
        public string? TravelerEmail { get; set; }
        public Traveler? Traveler { get; set; }

        public string? Role { get; set; } // Can be "Owner", "Editor", "Viewer" etc.
    }
}
