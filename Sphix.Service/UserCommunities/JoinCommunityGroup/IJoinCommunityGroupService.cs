using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.JoinCommunityGroup
{
   public interface IJoinCommunityGroupService
    {
        Task<BaseModel> JoinCommunityGroupAsync(JoinCommunityGroupViewModel model);
    }
}
