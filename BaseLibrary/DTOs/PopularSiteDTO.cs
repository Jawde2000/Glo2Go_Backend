using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class PopularSiteDTO
    {
        public string? SiteID { get; set; }
        public int? Rank { get; set; }
        public string? SiteName { get; set; }
        public int? ReviewCount { get; set; }
    }
}
