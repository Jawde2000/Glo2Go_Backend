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
                ReviewPics = review.ReviewPics
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
                    throw new Exception("No reviews found for the specified site.");  // Or return an empty list based on your application's needs

                var reviewDTOs = reviews.Select(review => new ReviewDTO
                {
                    ReviewTraveler = review.ReviewTraveler,
                    TravelerEmail = review.TravelerEmail,
                    ReviewSite = review.ReviewSite,
                    ReviewRating = review.ReviewRating,
                    ReviewPics = review.ReviewPics
                }).ToList();

                return reviewDTOs;
        }



        public async Task<ReviewResponse> UpdateSiteReviewAsync(UpdateReviewDTO review)
            {
                throw new NotImplementedException();
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
