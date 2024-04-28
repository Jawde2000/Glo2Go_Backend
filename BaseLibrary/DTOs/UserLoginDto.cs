using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class UserLoginDto : UserDto
    {
        public bool IsLocked {  get; set; }
    }
}
