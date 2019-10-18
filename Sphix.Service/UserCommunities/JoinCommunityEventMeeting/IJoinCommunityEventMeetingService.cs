using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.JoinCommunityEventMeeting
{
   public interface IJoinCommunityEventMeetingService
    {
        Task<BaseModel> SaveAsync(JoinCommunityOpenHoursMeetingViewModel model);
    }
}
