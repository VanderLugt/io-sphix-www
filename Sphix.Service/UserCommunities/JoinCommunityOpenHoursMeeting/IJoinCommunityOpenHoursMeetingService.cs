using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.JoinCommunityOpenHoursMeeting
{
    public interface IJoinCommunityOpenHoursMeetingService
    {
        Task<BaseModel> SaveAsync(JoinCommunityOpenHoursMeetingViewModel model);
    }
}
