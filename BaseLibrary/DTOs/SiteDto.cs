﻿namespace BaseLibrary.DTOs
{
    public class SiteDto
    {
        public string SiteID { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string SiteCountry { get; set; } = string.Empty;
        public string SiteAddress { get; set; } = string.Empty;
        public int SiteRating { get; set; } = 0;
        public List<string>? SitePics { get; set; }
        public string SiteDesc { get; set; } = string.Empty;
        public string SiteOperatingHour { get; set; } = string.Empty;
    }
}