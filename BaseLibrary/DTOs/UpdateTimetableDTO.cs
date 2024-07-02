using BaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class UpdateTimetableDTO
    {
        public string? TimelineTitle { get; set; }

        public string? Country { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? TimelineStartDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? TimelineEndDate { get; set; }

        public string? Region { get; set; }
    }
}
