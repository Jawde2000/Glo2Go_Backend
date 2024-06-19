using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class Activity
    {
        [Key]
        public string? ActivityID { get; set; }  // Primary key for the activity
        public string? ActivityTitle { get; set; }  // Title or name of the activity
        public DateTime? ActivityStart { get; set; }  // Duration of the activity in hours
        public DateTime? ActivityEnd { get; set; }  // Duration of the activity in hours

        // Activity type could be predefined types like sightseeing, dining, hiking, etc.
        public string? ActivityType { get; set; }
        public string? ActivityRegion { get; set; }

        // Additional properties for more complex activity descriptions or requirements
        public string? ActivityDescription { get; set; }  // Detailed description of the activity
        public string? TimetableID { get; set; }
    }
}
