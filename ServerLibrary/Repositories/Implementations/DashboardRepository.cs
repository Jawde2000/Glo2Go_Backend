using BaseLibrary.DTOs;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class DashboardRepository(Glo2GoDbContext dbContext) : IDashboard
    {

        public async Task<Dictionary<float, int>> GetReviewsCountByRatingAsync()
        {
            return await dbContext.Reviews
                .GroupBy(r => r.ReviewRating)
                .Select(group => new { Rating = group.Key, Count = group.Count() })
                .ToDictionaryAsync(g => g.Rating, g => g.Count);
        }

        public async Task<List<ReviewDTO>> GetRecentReviewsAsync(int count)
        {
            return await dbContext.Reviews
                .OrderByDescending(r => r.DateTime)
                .Take(count)
                .Select(r => new ReviewDTO
                {
                    ReviewTraveler = r.ReviewTraveler,
                    TravelerEmail = r.ReviewRating <= 3 ? "anonymous" : r.TravelerEmail,
                    ReviewSite = r.ReviewSite,
                    ReviewRating = r.ReviewRating,
                    ReviewPics = r.ReviewPics,
                    DateTime = r.DateTime,
                    ReviewID = r.ReviewID,
                    emailID = r.TravelerEmail
                })
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetReportsCountByTypeAsync()
        {
            return await dbContext.Reports
                .GroupBy(r => r.ReportType)
                .Select(group => new { Type = group.Key, Count = group.Count() })
                .ToDictionaryAsync(g => g.Type, g => g.Count);
        }

        public async Task<Dictionary<string, int>> GetReportsCountByApprovalStatusAsync()
        {
            return await dbContext.Reports
                .GroupBy(r => r.IsApproved)
                .Select(group => new { IsApproved = group.Key, Count = group.Count() })
                .ToDictionaryAsync(g => g.IsApproved ? "Approved" : "Not Approved", g => g.Count);
        }

        public async Task<List<ReportDTO>> GetRecentReportsAsync(int count)
        {
            return await dbContext.Reports
                .OrderByDescending(r => r.ReportId)
                .Take(count)
                .Select(r => new ReportDTO
                {
                    ReportId = r.ReportId,
                    ReportTitle = r.ReportTitle,
                    ReportFeedback = r.ReportFeedback,
                    ReportType = r.ReportType,
                    SiteID = r.SiteID,
                    IsApproved = r.IsApproved,
                    IsReviewedByAdmin = r.IsReviewedByAdmin,
                })
                .ToListAsync();
        }

        public async Task<int> GetTotalActivitiesCountAsync()
        {
            return await dbContext.Activities.CountAsync();
        }

        public async Task<int> GetTotalReportsCountAsync()
        {
            return await dbContext.Reports.CountAsync();
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            return await dbContext.Reviews.CountAsync();
        }

        public async Task<Dictionary<string, int>> GetActivitiesCountByTypeAsync()
        {
            return await dbContext.Activities
                .GroupBy(a => a.ActivityType)
                .Select(group => new { Type = group.Key, Count = group.Count() })
                .ToDictionaryAsync(g => g.Type, g => g.Count);
        }

        public async Task<double> GetAverageReviewRatingAsync()
        {
            return await dbContext.Reviews.AverageAsync(r => r.ReviewRating);
        }

        public async Task<List<string>> GetMostActiveUsersAsync(int count)
        {
            return await dbContext.Reviews
                .GroupBy(r => r.TravelerEmail)
                .OrderByDescending(group => group.Count())
                .Take(count)
                .Select(group => group.Key)
                .ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetReportsCountBySiteAsync()
        {
            return await dbContext.Reports
                .GroupBy(r => r.SiteID)
                .Select(group => new { SiteID = group.Key, Count = group.Count() })
                .ToDictionaryAsync(g => g.SiteID, g => g.Count);
        }

        public async Task<int> GetTotalSitesCountAsync()
        {
            return await dbContext.Sites.CountAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await dbContext.Travelers.CountAsync();
        }

        public async Task<List<ReportDTO>> GetLatestReportsAsync(int count)
        {
            var reports = await dbContext.Reports
                .OrderByDescending(r => r.ReportId) // Assuming ReportId is incremental
                .Take(count)
                .ToListAsync();

            if (reports == null || reports.Count == 0)
            {
                return new List<ReportDTO>();
            }

            var reportDTOs = reports.Select(report => new ReportDTO
            {
                ReportId = report.ReportId,
                ReportTitle = report.ReportTitle,
                ReportFeedback = report.ReportFeedback,
                ReportType = report.ReportType,
                SiteID = report.SiteID,
                ReportEmail = report.ReportEmail,
                IsApproved = report.IsApproved,
                IsReviewedByAdmin = report.IsReviewedByAdmin,
            }).ToList();

            return reportDTOs;
        }

/*        public async Task<List<PopularSiteDTO>> GetMostPopularSitesAsync(int count = 5)
        {
            return await dbContext.Reviews
                .GroupBy(r => r.ReviewSite)
                .OrderByDescending(group => group.Count())
                .Take(count)
                .Select(group => new PopularSiteDTO
                {
                    SiteID = group.Key,
                    SiteName = dbContext.Sites.FirstOrDefault(s => s.SiteID == group.Key).SiteName,
                    ReviewCount = group.Count()
                })
                .ToListAsync();
        }
*/
        public async Task<List<PopularSiteDTO>> GetMostPopularSitesAsync(int count = 5)
        {
            var popularSites = await dbContext.Reviews
                .GroupBy(r => r.ReviewSite)
                .OrderByDescending(group => group.Count())
                .Take(count)
                .Select(group => new PopularSiteDTO
                {
                    SiteID = group.Key,
                    SiteName = dbContext.Sites.FirstOrDefault(s => s.SiteID == group.Key).SiteName,
                    ReviewCount = group.Count()
                })
                .ToListAsync();

            // Assign ranks after retrieving data
            for (int i = 0; i < popularSites.Count; i++)
            {
                popularSites[i].Rank = i + 1;
            }

            return popularSites;
        }

    }
}
