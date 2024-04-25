using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class UserRegisterDto : UserDto
    {
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string? ConfirmPass { get; set; } = string.Empty;
    }
}
