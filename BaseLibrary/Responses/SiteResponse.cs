using System.Collections.Specialized;

namespace BaseLibrary.Responses
{
    public record SiteResponse(bool Flag, string Message = null!, string data = null!);

/*    public record SiteData
    {
        public string SiteID { get; init; } = string.Empty;
        public string SiteName { get; init; } = string.Empty;
        public string SiteDesc { get; init; } = string.Empty;
        public string SiteOperatingHour { get; init; } = string.Empty;
        public List<string>? SitePics { get; init; }
        public int SiteRating { get; init; } = 0;
    }*/
}
