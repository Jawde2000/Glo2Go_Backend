using BaseLibrary.DTOs;
using BaseLibrary.Models;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Repositories.Contracts;
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
                SiteDesc = addSite.SiteDesc,
                SiteOperatingHour = addSite.SiteOperatingHour,
                SitePics = addSite.SitePics,
                SiteRating = addSite.SiteRating,
            };

            // Run FindSiteById and AddToDB in parallel
            var existingSiteTask = FindSiteById(siteId);
            var addSiteTask = AddToDB(newSite);
            await Task.WhenAll(existingSiteTask, addSiteTask);

            if (existingSiteTask != null) return new SiteResponse(true, "Congratulations! Your site has been successfully created.");

            throw new NotImplementedException();
        }


        public Task<SiteResponse> DeleteSiteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SiteResponse> UpdateSiteAsync(UpdateSiteDTO site)
        {
            throw new NotImplementedException();
        }

        private async Task<Site> FindSiteName(string siteName)
        {
            return await dbContext.Sites.FirstOrDefaultAsync(_ => _.SiteName!.ToLower()!.Equals(siteName!.ToLower()));
        }
        public async Task<string> GetNextId()
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

        public Task<SiteResponse> DeleteSiteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
