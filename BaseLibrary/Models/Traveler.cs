using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class Traveler
    {
        public string? Name { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? ProfilePic { get; set; }

        [Key]
        public string? TravelerEmail { get; set;}
        public string? TravelerPass { get; set; }

        public Gender? Gender { get; set; }

        public bool? IsLocked { get; set; } = false;
        public int FailedLoginAttempt { get; set; } = 0;

        public ICollection<TimetableCollaborator> Timetables { get; set; } = new List<TimetableCollaborator>();
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
