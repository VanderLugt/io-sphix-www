using Sphix.DataModels.User;
using Sphix.DataModels.UserCommunities;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.OpenOfficeHours
{
    public interface IOpenOfficeHoursService
    {
        Task<CommunityOpenOfficeHours> getOpenHoursAsync(long Id);
        Task<BaseModel> SaveOpenHoursAsync(OpenOfficeHoursViewModel model, UsersLoginDataModel user, CommunityGroupsDataModel communityGroupsData);
        Task<BaseModel> CheckTableIsExist(long userId, long communityId, string TimeZone, DateTime fromDateTime);
        Task<CommunityOpenOfficeHours> getOpenHoursAsync(string token);
        Task<BaseModel> UpdateNextMeetingToken(long Id);
    }
}
