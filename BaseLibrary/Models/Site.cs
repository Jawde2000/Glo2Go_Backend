namespace BaseLibrary.Models
{
    public class Site
    {
        public string SiteID { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string SiteDesc { get; set; } = string.Empty;
        public string SiteOperatingHour { get; set; } = string.Empty;

        public int SiteRating { get; set; } = 0;
    }
}
