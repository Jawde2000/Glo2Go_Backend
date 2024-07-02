using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ReportDTO
    {
        [Key]
        public int? ReportId { get; set; }
        [Required]
        public string? SiteID { get; set; }

        [Required]
        public string? ReportTitle { get; set; }

        [Required]
        public string? ReportFeedback { get; set; }

        [Required]
        public string? ReportType { get; set; }
        public string? ReportEmail { get; set; }

        public bool IsApproved { get; set; } = false;

        public bool IsReviewedByAdmin { get; set; } = false;
    }
}
