using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class Activity
    {
        [Key]
        public string? ActivityID { get; set; }  // Primary key for the activity
        public string? ActivityTitle { get; set; }  // Title or name of the activity
        public double? ActivityDuration { get; set; }  // Duration of the activity in hours

        // Activity type could be predefined types like sightseeing, dining, hiking, etc.
        public string? ActivityType { get; set; }

        // Foreign Key - Timetable
        [ForeignKey("Timetable")]
        public string? TimelineID { get; set; }  // Link to the Timetable
        public Timetable? Timetable { get; set; }  // Navigation property

        // Additional properties for more complex activity descriptions or requirements
        public string? Description { get; set; }  // Detailed description of the activity
        public DateOnly? ActivityDate { get; set; }  // Specific date for the activity within the timetable
    }
}
