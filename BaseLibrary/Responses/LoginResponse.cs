using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Responses
{
    public record LoginResponse(bool Flag, String Message = null!, string Token = null!, string RefreshToken = null!);
}
