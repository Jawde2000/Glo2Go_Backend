using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Models
{
    public class Address
    {
        [Key]
        public int? AddressId { get; set; }  // Primary key for the Address entity
        public string? TravelAddress { get; set; }
        public string? Country { get; set; }

        // Foreign Key property as a string
        [Required]
        public string? TravelerEmail { get; set; }

        // Navigation property
        [ForeignKey("TravelerEmail")]
        public Traveler? Traveler { get; set; }
    }
}
