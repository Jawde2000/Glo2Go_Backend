namespace BaseLibrary.DTOs
{
    public class ReviewDTO
    {
        public string ReviewTraveler { get; set; } = string.Empty;
        public string? TravelerEmail { get; set; }

        public string? ReviewSite { get; set; }
        public float ReviewRating { get; set; } = 0;
        public List<string>? ReviewPics { get; set; }
    }
}
