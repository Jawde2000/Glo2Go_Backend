using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using ServerLibrary.Repositories.Contracts;
using BaseLibrary.Models;
using static BaseLibrary.Responses.ReviewSiteResponse;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ServerLibrary.Repositories.Implementations
{
    public class ReviewRepository(Glo2GoDbContext dbContext) : ISiteReview
    {
        public async Task<GeneralResponse> AddSiteReviewAsync(ReviewDTO review)
        {
            if (review == null) return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var newReview = new Review()
            {
                ReviewTraveler = review.ReviewTraveler,
                ReviewSite = review.ReviewSite,
                ReviewRating = review.ReviewRating,
                TravelerEmail = review.TravelerEmail,
                ReviewPics = review.ReviewPics,
                DateTime = DateTime.UtcNow
            };

            var addReview = AddToDB(newReview);


            if (addReview != null) return new GeneralResponse(true, "Thank you! Your review was successfully submitted.");
            throw new NotImplementedException();
        }

        public async Task<GeneralResponse> DeleteSiteReviewAsync(DeleteReviewDTO deleteReview)
        {
            var review = await dbContext.Reviews.FindAsync(deleteReview.ReviewID);
            if (review == null)
            {
                return new GeneralResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");
            }

            dbContext.Reviews.Remove(review);
            await dbContext.SaveChangesAsync();
            return new GeneralResponse(true, "Review deleted successfully.");
        }

        public async Task<List<ReviewDTO>> GetSiteReviewAsync(ViewReviewDTO reviewSite)
        {
            if (string.IsNullOrWhiteSpace(reviewSite.ReviewSite))
                throw new ArgumentException("ReviewSite must be provided.");

            // Fetching reviews filtered by ReviewSite
            var reviews = await dbContext.Reviews
                .Where(r => r.ReviewSite == reviewSite.ReviewSite)
                .ToListAsync();

            if (reviews.Count == 0)
                return new List<ReviewDTO>();

            // Identify the traveler's own comment
            var travelerCom = reviews.FirstOrDefault(r => r.TravelerEmail == reviewSite.TravelerComment);
            if (travelerCom != null)
            {
                reviews.Remove(travelerCom);  // Remove it from the list so it can be added on top later
            }

            var reviewDTOs = reviews.Select(review => new ReviewDTO
            {
                ReviewTraveler = review.ReviewTraveler,
                TravelerEmail = review.ReviewRating <= 3 ? "anonymous" : review.TravelerEmail,
                ReviewSite = review.ReviewSite,
                ReviewRating = review.ReviewRating,
                ReviewPics = review.ReviewPics,
                DateTime = review.DateTime,
                ReviewID = review.ReviewID,
                emailID = review.TravelerEmail
            }).ToList();

            // Add the traveler's own comment to the top of the list, if it exists
            if (travelerCom != null)
            {
                var travelerReviewDTO = new ReviewDTO
                {
                    ReviewTraveler = travelerCom.ReviewTraveler,
                    TravelerEmail = travelerCom.ReviewRating <= 3 ? "anonymous" : travelerCom.TravelerEmail,
                    ReviewSite = travelerCom.ReviewSite,
                    ReviewRating = travelerCom.ReviewRating,
                    ReviewPics = travelerCom.ReviewPics,
                    DateTime = travelerCom.DateTime,
                    ReviewID = travelerCom.ReviewID,
                    emailID = travelerCom.TravelerEmail,
                };
                reviewDTOs.Add(travelerReviewDTO);  // Insert at the beginning
            }

            return reviewDTOs;
        }

        public async Task<ReviewResponse> UpdateSiteReviewAsync(UpdateReviewDTO review)
        {
            // Assume dbContext is already defined in your service
            // Find the review by ID
            var siteReview = await dbContext.Reviews
                .FirstOrDefaultAsync(r => r.ReviewID == review.ReviewID);

            if (siteReview == null)
            {
                return new ReviewResponse(false, "Review not found. Please check the Review ID and try again.");
            }

            // Update review properties
            siteReview.ReviewTraveler = review.ReviewTraveler ?? siteReview.ReviewTraveler;
            siteReview.TravelerEmail = review.TravelerEmail ?? siteReview.TravelerEmail;
            siteReview.ReviewRating = review.ReviewRating != 0 ? review.ReviewRating : siteReview.ReviewRating;

            // Update review pictures if they are provided
            if (review.ReviewPics != null && review.ReviewPics.Count > 0)
            {
                siteReview.ReviewPics = review.ReviewPics;
            }

            // Save the changes to the database
            dbContext.Reviews.Update(siteReview);
            await dbContext.SaveChangesAsync();

            return new ReviewResponse(true, "Review updated successfully.");
        }

        public async Task<ReviewResponse> GetRandomSiteReviewAsync()
        {
            // Fetch all reviews including related site data
            var reviewsWithSites = await dbContext.Reviews
                .Include(r => r.Site) // Make sure this includes all necessary site fields
                .ToListAsync();

            if (reviewsWithSites == null || reviewsWithSites.Count == 0)
                return new ReviewResponse(false, "No reviews available.");

            // Shuffle reviews and take the first 5
            Random rnd = new Random();
            var randomReviews = reviewsWithSites.OrderBy(x => rnd.Next()).Take(5).ToList();

            // Prepare details for each review
            var reviewsDetails = randomReviews.Select(review => new
            {
                ReviewID = review.ReviewID,
                ReviewTraveler = review.ReviewTraveler,
                ReviewSite = review.ReviewSite,
                TravelerEmail = review.ReviewRating <= 3 ? "anonymous" : review.TravelerEmail,
                ReviewRating = review.ReviewRating,
                ReviewPics = review.ReviewPics,
                SiteDetails = review.Site != null ? new
                {
                    SiteID = review.Site.SiteID,
                    SiteName = review.Site.SiteName,
                    SiteCountry = review.Site.SiteCountry,
                    SiteAddress = review.Site.SiteAddress,
                    SiteDesc = review.Site.SiteDesc,
                    SitePics = review.Site.SitePics,
                    SiteRating = review.Site.SiteRating,
                    SiteOperatingHours = review.Site.SiteOperatingHour 
                } : null
            }).ToList();

            var jsonReviewDetails = JsonConvert.SerializeObject(reviewsDetails, Formatting.Indented);
            return new ReviewResponse(true, jsonReviewDetails);
        }


        public async Task<ReviewResponse> ViewSitesReviewAsync() //view all
        {
            var Viewsite = await dbContext.Reviews.ToListAsync();

            if (Viewsite == null || Viewsite.Count == 0) return new ReviewResponse(false, "No sites found.");

            var jsonSites = JsonConvert.SerializeObject(Viewsite, Newtonsoft.Json.Formatting.Indented);

            return new ReviewResponse(true, jsonSites);
        }

        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}
