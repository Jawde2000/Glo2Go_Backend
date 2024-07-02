using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IDashboard
    {
        public Task<Dictionary<float, int>> GetReviewsCountByRatingAsync();
        public Task<List<ReviewDTO>> GetRecentReviewsAsync(int count);
        public Task<Dictionary<string, int>> GetReportsCountByTypeAsync();
        public Task<Dictionary<string, int>> GetReportsCountByApprovalStatusAsync();
        public Task<List<ReportDTO>> GetRecentReportsAsync(int count);
        public Task<int> GetTotalActivitiesCountAsync();
        public Task<int> GetTotalReportsCountAsync();
        public Task<int> GetTotalReviewsCountAsync();
        public Task<Dictionary<string, int>> GetActivitiesCountByTypeAsync();
        Task<double> GetAverageReviewRatingAsync();
        Task<List<string>> GetMostActiveUsersAsync(int count);
        Task<Dictionary<string, int>> GetReportsCountBySiteAsync();
        public Task<int> GetTotalSitesCountAsync();
        public Task<int> GetTotalUsersCountAsync();
        public Task<List<ReportDTO>> GetLatestReportsAsync(int count);
        public Task<List<PopularSiteDTO>> GetMostPopularSitesAsync(int count);
    }
}
