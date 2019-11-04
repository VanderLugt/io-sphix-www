using Sphix.DataModels.Communities;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.Communities.ComunitySubTypes
{
    public interface IComunitySubTypesService
    {
        Task<CommunitySubTypesViewModel> getSubCategorybyIdAsync(int Id);
        Task<IEnumerable<CommunityCatgoryDataModel>> getSubCategoreisbyTypeAsync(int communityId);
        Task<BaseModel> SaveAsync(CommunitySubTypesViewModel model);
    }
}
