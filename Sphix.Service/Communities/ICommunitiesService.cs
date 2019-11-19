using Microsoft.AspNetCore.Http;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;
using Sphix.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.Communities
{
   public interface ICommunitiesService
    {
        Task<IList<SelectListItems>> GetActiveCommunities();
        Task<SignUpStep3ViewModel> GetActiveSubCommunities(int id);
        Task<IList<SelectListItems>> GetActiveCommunityThemes(int id);
        Task<CommunityTypeViewModel> getCommunityTypeAsync(int id);
        Task<IList<CommunityTypesListViewModel>> getCommunitiesGroupsTypeList(CustomeSearchFilter model);
        Task<BaseModel> SaveAsync(CommunityTypeViewModel model, IFormFile articleShareDocument, IFormFile headerLogo);
    }
}
