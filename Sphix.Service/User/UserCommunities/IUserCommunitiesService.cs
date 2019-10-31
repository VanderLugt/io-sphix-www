using Sphix.DataModels.Communities;
using Sphix.DataModels.User;
using Sphix.ViewModels;
using System.Threading.Tasks;

namespace Sphix.Service.User.UserCommunities
{
  public interface IUserCommunitiesService
    {
        Task<bool> SaveCommunitiesAsync(string communitiesIds, UsersLoginDataModel model);
        Task<bool> SaveSubCommunitiesAsync(UserSubCommunitiesDataModel model);
        Task<BaseModel> JoinCommunitiesAsync(long communitieId, long UserId);
        Task<bool> SaveGroupsAsync(string groupIds, UsersLoginDataModel user, CommunityDataModel community);
        Task<bool> SaveInterestsAsync(string interestsIds, UsersLoginDataModel model, CommunityDataModel community);
        Task<bool> SaveAssociationsAsync(string associationsIds, UsersLoginDataModel user, CommunityDataModel community);
    }
}
