using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class Traveler
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? ProfilePic { get; set; }

        public Address Address { get; set; } // This is the new Address property

        [Key]
        public string? TravelerEmail { get; set;}
        public string? TravelerPass { get; set; }

        public Gender? Gender { get; set; }

        public bool? IsLocked { get; set; } = false;
        public int FailedLoginAttempt { get; set; } = 0;

        // [Key]
        // [Required(ErrorMessage = "Email is required")]
        // [EmailAddress(ErrorMessage = "Invalid email address")]
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
