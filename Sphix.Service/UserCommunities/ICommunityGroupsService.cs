using Microsoft.AspNetCore.Http;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities
{
   public interface ICommunityGroupsService 
    {
        Task<BaseModel> SaveAsync(CommunityGroupViewModel model, IFormFile file, IFormFile aticleSharedoc);
        Task<BaseModel> PublishCommunityGroupAsync(long Id,bool IsPublish);
        Task<IList<CommunityGroupsListViewModel>> getCommunitiesGroupsList(SearchFilter model);
        
        Task<IList<CommunityGroupsListViewModel>> getAdminCommunitiesGroupsList(CustomeSearchFilter model);
        Task<EditCommunityGroupViewModel> getCommunityGroupDetail(long Id, long UserId);
        Task<IList<ViewCommunityGroupViewModel>> getCommunityGroupDetailAdmin(long Id);
    }
}
