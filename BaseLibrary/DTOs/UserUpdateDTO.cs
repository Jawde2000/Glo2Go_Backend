using BaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class UserUpdateDTO
    {
        public string? Name { get; set; }
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? ProfilePic { get; set; }

        [Key]
        public string? TravelerEmail { get; set; }
        public string? TravelerPass { get; set; }

        public Gender? Gender { get; set; }
    }
}
