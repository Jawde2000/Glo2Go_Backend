using BaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using ServerLibrary;
using ServerLibrary.Repositories.Implementations;

public class SearchService(Glo2GoDbContext dbContext)
{

    public SearchResultDto CombinedSearch(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            throw new ArgumentException("Search term cannot be null or empty", nameof(searchTerm));

        List<Review> reviews = SearchReviews(searchTerm);
        List<Site> sites = SearchSites(searchTerm);

        return new SearchResultDto
        {
            Reviews = reviews,
            Sites = sites
        };
    }
    public List<Review> SearchReviews(string searchTerm)
    {
        var reviews = dbContext.Reviews
            .Where(r => EF.Functions.Like(r.ReviewTraveler, $"%{searchTerm}%") ||
                        EF.Functions.Like(r.ReviewSite, $"%{searchTerm}%") ||
                        EF.Functions.Like(r.TravelerEmail, $"%{searchTerm}%"))
            .ToList();
        return reviews;
    }

    public List<Site> SearchSites(string searchTerm)
    {
        var sites = dbContext.Sites
            .Where(s => EF.Functions.Like(s.SiteName, $"%{searchTerm}%") ||
                        EF.Functions.Like(s.SiteCountry, $"%{searchTerm}%") ||
                        EF.Functions.Like(s.SiteFee, $"%{searchTerm}%") ||
                        EF.Functions.Like(s.SiteAddress, $"%{searchTerm}%") ||
                        EF.Functions.Like(s.SiteDesc, $"%{searchTerm}%"))  // Adjust fields as per your schema
            .ToList();
        return sites;
    }
}

public class SearchResultDto
{
    public List<Review> Reviews { get; set; }
    public List<Site> Sites { get; set; }
}


