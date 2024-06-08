using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.Models
{
    public class TimetableRegions
    {
        [Key]
        public int RegionID { get; set; }
        public string? TimelineID { get; set; }
        public DateOnly? DateStart { get; set; }
        public DateOnly? DateEnd { get; set; }
        public TimeOnly? TimeStart { get; set; }
        public TimeOnly? TimeEnd { get; set; }
        public string? RegionName { get; set; }
    }
}
