namespace BaseLibrary.DTOs
{
    public class AddSiteDto
    {
        public string SiteName { get; set; } = string.Empty;
        public string SiteCountry { get; set; } = string.Empty;
        public string SiteAddress { get; set; } = string.Empty;
        public string SiteFee { get; set; } = string.Empty;
        public int SiteRating { get; set; } = 0;
        public List<string>? SitePics { get; set; }
        public string SiteDesc { get; set; } = string.Empty;
        public string SiteOperatingHour { get; set; } = string.Empty;
    }
}
