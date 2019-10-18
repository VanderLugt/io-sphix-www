using Sphix.ViewModels;
using Sphix.ViewModels.Communities;
using Sphix.ViewModels.CommunityGroupsFroentEnd;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.CommunitiesForGood
{
   public interface ICommunitiesForGoodService
    {
        Task<IList<CommunityForGoodList>> getCommunitiesForGood(long UserId);
        Task<IList<CommunityGroupsFroentEndDataView>> getCommunitiesByCategoryIdAsync(SearchFilter model);
        Task<CmmunityGroupDetailViewModel> getCommunityGroupDetail(long Id);
        Task<IList<CommunityGroupEventsListViewModel>> getActiveCommunityGroupEventsListAsync(EventListSearchFilter model);
        Task<IList<CommunityGroupArticlesList>> getActiveCommunityGroupArticlesListAsync(EventListSearchFilter model);
        Task<OpenOfficeHoursViewModel> getCommunityGroupOpenHoursDetail(long Id);
        Task<LiveEventViewModel> getCommunityGroupEventDetail(long Id);
        Task<ArticleViewModel> getArticleDetailAsync(long Id);
        Task<IList<OpenOfficeHoursTables>> getOpenOfficeHoursTables(long CommunityGroupsId,string TimeZone);
    }
}
