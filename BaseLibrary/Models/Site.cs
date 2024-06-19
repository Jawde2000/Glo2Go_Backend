namespace BaseLibrary.Models
{
    public class Site
    {
        public string SiteID { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string SiteCountry { get; set; } = string.Empty;
        public string SiteFee { get; set; } = string.Empty;
        public string SiteAddress { get; set; } = string.Empty;
        public string SiteDesc { get; set; } = string.Empty;
        public string SiteOperatingHour { get; set; } = string.Empty;

        public List<string>? SitePics { get; set; }

        public float SiteRating { get; set; } = 0;

        public List<Review> Reviews { get; set; } = new List<Review>();

    }
}
