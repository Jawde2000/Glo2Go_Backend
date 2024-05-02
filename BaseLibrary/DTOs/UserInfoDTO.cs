using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class UserInfoDTO
    {
        [Key]
        public string? TravelerEmail { get; set; }
    }
}
