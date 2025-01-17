﻿using BaseLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class CreateTimetableDTO 
    {
        public string? TimelineTitle { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? TimelineStartDate { get; set; }
        [DataType(DataType.Date)]
        public DateOnly? TimelineEndDate { get; set; }

        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? TravelerEmail { get; set; }
        public List<string>? CollaboratorEmails { get; set; }  // List of emails for collaborators
    }
}
