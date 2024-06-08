using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class ActivityDTO
    {
        public string? ActivityID { get; set; }
        public string? ActivityTitle { get; set; }
        public double? ActivityStartTime { get; set; }
        public double? ActivityEndTime { get; set; }
        public string? ActivityType { get; set; }
        public DateOnly? ActivityDate { get; set; }
        public string? TimelineID { get; set; }  // Link to the Timetable
    }
}
