using BaseLibrary.DTOs;
using BaseLibrary.Responses;
using static BaseLibrary.Responses.ReviewSiteResponse;

namespace ServerLibrary.Repositories.Contracts
{
    public interface ISiteReview
    {
        public Task<GeneralResponse> AddSiteReviewAsync(ReviewDTO review);
        public Task<ReviewResponse> UpdateSiteReviewAsync(UpdateReviewDTO review);
        public Task<GeneralResponse> DeleteSiteReviewAsync(DeleteReviewDTO SiteID);

        public Task<List<ReviewDTO>> GetSiteReviewAsync(ViewReviewDTO reviewSite);
        public Task<ReviewResponse> ViewSitesReviewAsync();
    }
}
