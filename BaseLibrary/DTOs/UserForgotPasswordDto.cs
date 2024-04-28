using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class UserForgotPasswordDto
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
    }
}
