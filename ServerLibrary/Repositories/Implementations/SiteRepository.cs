using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServerLibrary.Repositories.Contracts;
using System.Xml;
namespace ServerLibrary.Repositories.Implementations
{
    public class SiteRepository(Glo2GoDbContext dbContext) : ISiteAccount
    {
        public async Task<SiteResponse> AddSiteAsync(AddSiteDto addSite)
        {
            if (addSite == null) return new SiteResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            // Generate the SiteID
            string siteId = await GetNextId(); // This method would get the next ID from your database

            // Prepare the new site
            var newSite = new Site()
            {
                SiteID = siteId,
                SiteName = addSite.SiteName,
                SiteCountry = addSite.SiteCountry,
                SiteAddress = addSite.SiteAddress,
                SiteFee = addSite.SiteFee,
                SiteDesc = addSite.SiteDesc,
                SiteOperatingHour = addSite.SiteOperatingHour,
                SitePics = addSite.SitePics,
                SiteRating = addSite.SiteRating,
            };

            var addSiteTask = AddToDB(newSite);

            if (addSiteTask != null) return new SiteResponse(true, "Congratulations! Your site has been successfully created.");

            throw new NotImplementedException();
        }


        public async Task<SiteResponse> DeleteSiteAsync(DeleteSiteDTO deleteSite)
        {
            if (deleteSite == null) return new SiteResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var site = await FindSiteById(deleteSite.SiteID);
            if (site == null) return new SiteResponse(false, "The site with the given ID does not exist.");

            dbContext.Sites.Remove(site);
            await dbContext.SaveChangesAsync();

            return new SiteResponse(true, "The site has been successfully deleted.");
        }

        public async Task<SiteResponse> UpdateSiteAsync(UpdateSiteDTO updateSite)
        {
            if (updateSite == null) return new SiteResponse(false, "Heads up! The model currently contains no data. Please load or input data to proceed.");

            var site = await FindSiteById(updateSite.SiteID);
            if (site == null) return new SiteResponse(false, "The site with the given ID does not exist.");

            site.SiteName = updateSite.SiteName;
            site.SiteCountry = updateSite.SiteCountry;
            site.SiteAddress = updateSite.SiteAddress;
            site.SiteDesc = updateSite.SiteDesc;
            site.SiteOperatingHour = updateSite.SiteOperatingHour;
            site.SitePics = updateSite.SitePics;
            site.SiteRating = updateSite.SiteRating;

            dbContext.Sites.Update(site);
            await dbContext.SaveChangesAsync();

            return new SiteResponse(true, "The site has been successfully updated.");
        }


        private async Task<Site> FindSiteName(string siteName)
        {
            return await dbContext.Sites.FirstOrDefaultAsync(_ => _.SiteName!.ToLower()!.Equals(siteName!.ToLower()));
        }
        private async Task<string> GetNextId()
        {
            // Get the highest current numeric ID
            var siteIds = await dbContext.Sites
            .Select(s => s.SiteID.Substring(4)) // Skip the first 4 characters ("G2GS")
            .ToListAsync();

            var maxId = siteIds
                .Select(id => int.TryParse(id, out var numericId) ? numericId : (int?)null)
                .Max();

            // If there are no sites yet, start at 1, otherwise increment the max ID
            int nextNumericId = (maxId ?? 0) + 1;

            // Convert the numeric ID to a string, pad it with leading zeros to be 8 digits long,
            // and prepend "G2GS"
            string nextId = "G2GS" + nextNumericId.ToString("D8");

            return nextId;
        }


        private async Task<Site> FindSiteById(string siteID)
        {
            return await dbContext.Sites.FirstOrDefaultAsync(_ => _.SiteID!.Equals(siteID!));
        }
        private async Task<T> AddToDB<T>(T model)
        {
            var result = dbContext.Add(model!);
            await dbContext.SaveChangesAsync();
            return (T)result.Entity;
        }

        public async Task<SiteResponse> ViewSitesAsync()
        {
            var sites = await dbContext.Sites.ToListAsync();

            if (sites == null || sites.Count == 0) return new SiteResponse(false, "No sites found.");

            var jsonSites = JsonConvert.SerializeObject(sites, Newtonsoft.Json.Formatting.Indented);

            return new SiteResponse(true, jsonSites);
        }

        public async Task<SiteResponse> GetSiteAsync(SiteDetailsDTO site)
        {
            var siteDetails = await FindSiteById(site.SiteID);

            if (siteDetails == null) return new SiteResponse(false, "No sites found.");

            var details = JsonConvert.SerializeObject(siteDetails, Newtonsoft.Json.Formatting.Indented);

            return new SiteResponse(true, details);
        }

        public async Task<SiteResponse> GetTop3PopularSitesAsync()
        {
            // Fetch all reviews including related site data
            var reviewsWithSites = await dbContext.Reviews
                .Include(r => r.Site) // Include the related site information
                .ToListAsync();

            // Calculate the popularity score for each site based on its reviews
            var sitePopularityScores = reviewsWithSites.GroupBy(r => r.Site)
                .Select(group => new
                {
                    Site = group.Key,
                    PopularityScore = group.Average(r => r.ReviewRating) * group.Count()
                });

            // Sort sites by popularity score in descending order and take the top 3
            var top3Sites = sitePopularityScores.OrderByDescending(sp => sp.PopularityScore)
                .Take(3)
                .ToList();

            if (top3Sites == null || top3Sites.Count == 0)
            {
                return new SiteResponse(false, "No popular sites found.");
            }

            // Fetch detailed site information for the top 3 popular sites
            var detailedSites = await dbContext.Sites
                .Where(s => top3Sites.Select(sp => sp.Site.SiteID).Contains(s.SiteID))
                .ToListAsync();

            // Map popularity scores to the detailed sites
            var detailedSitesWithPopularity = detailedSites.Select(site => new
            {
                Site = site,
                PopularityScore = top3Sites.First(sp => sp.Site.SiteID == site.SiteID).PopularityScore
            }).ToList();

            // Configure JsonSerializerSettings to handle self-referencing loops
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var jsonPopularSites = JsonConvert.SerializeObject(detailedSites, Newtonsoft.Json.Formatting.Indented, settings);
            return new SiteResponse(true, jsonPopularSites);
        }

        public async Task<SiteResponse> GetRecommendedSitesAsync()
        {
            // Fetch all sites
            var allSites = await dbContext.Sites.ToListAsync();

            if (allSites == null || allSites.Count == 0)
            {
                return new SiteResponse(false, "No sites found.");
            }

            // Placeholder logic for diversity: fetch random sites to ensure variety
            var recommendedSites = allSites.OrderBy(s => Guid.NewGuid()).Take(3).ToList();

            var jsonRecommendedSites = JsonConvert.SerializeObject(recommendedSites, Newtonsoft.Json.Formatting.Indented);
            return new SiteResponse(true, jsonRecommendedSites);
        }

    }
}
