namespace BaseLibrary.DTOs
{
    public class ReviewDTO
    {
        public int? ReviewID { get; set; }
        public string ReviewTraveler { get; set; } = string.Empty;
        public string? TravelerEmail { get; set; }
        public string? emailID { get; set; }

        public string? ReviewSite { get; set; }
        public float ReviewRating { get; set; } = 0;
        public List<string>? ReviewPics { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Site { get; set; }
    }
}
