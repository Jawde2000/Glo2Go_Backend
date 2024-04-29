using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Responses
{
    public class ReviewSiteResponse
    {
        public record ReviewResponse(bool Flag, String Message = null!, string data = null!);
    }
}
