// Models/Report.cs
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

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
