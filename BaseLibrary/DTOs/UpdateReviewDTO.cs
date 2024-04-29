namespace BaseLibrary.DTOs
{
    public class UpdateReviewDTO
    {
        public string ReviewID { get; set; } = string.Empty;
        public string ReviewTraveler { get; set; } = string.Empty;
        public string? TravelerEmail { get; set; }
        public float ReviewRating { get; set; } = 0;
        public List<string>? ReviewPics { get; set; }
    }
}
