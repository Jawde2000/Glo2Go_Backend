﻿namespace BaseLibrary.Models
{
    public class Review
    {
        public int ReviewID { get; set; }
        public string ReviewTraveler { get; set; } = string.Empty;
        public string ReviewSite {  get; set; } = string.Empty;
        public string? TravelerEmail { get; set; }
        public float ReviewRating { get; set; } = 0;
        public List<string>? ReviewPics { get; set; }

    }
}
