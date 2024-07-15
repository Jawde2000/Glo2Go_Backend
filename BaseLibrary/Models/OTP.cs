using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class OTP
    {
        [Key]
        public int otp { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
