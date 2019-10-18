using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
  public interface ICommunityGroupEventService
    {
        Task<BaseModel> SaveEventAsync(LiveEventViewModel model);
        Task<IList<CommunityGroupEventsListViewModel>> getUserCommunityGroupEventsListAsync(EventListSearchFilter model);
        Task<LiveEventViewModel> getEventDetailAsync(long eventId);
    }
}
