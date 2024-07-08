using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.DTOs
{
    public class WeatherDTO
    {
        [DataType(DataType.Date)]
        public DateOnly? startDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? endDate { get; set; }
        public string? region { get; set; }
        public string? country { get; set; }
    }
}
