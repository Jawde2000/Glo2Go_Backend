using BaseLibrary.DTOs;
using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface ISiteAccount
    {
        public Task<SiteResponse> AddSiteAsync(AddSiteDto addSite);
        public Task<SiteResponse> UpdateSiteAsync(UpdateSiteDTO site);
        public Task<SiteResponse> DeleteSiteAsync(DeleteSiteDTO SiteID);

        public Task<SiteResponse> GetSiteAsync(SiteDetailsDTO site);
        public Task<SiteResponse> ViewSitesAsync();
        public Task<SiteResponse> GetTop3PopularSitesAsync();
    }
}
