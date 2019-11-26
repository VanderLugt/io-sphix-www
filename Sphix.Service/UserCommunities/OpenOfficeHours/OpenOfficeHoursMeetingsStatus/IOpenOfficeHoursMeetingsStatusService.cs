using Sphix.DataModels.UserCommunitiesGroups;
using Sphix.ViewModels;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursMeetingsStatus
{
    public interface IOpenOfficeHoursMeetingsStatusService
    {
        Task<BaseModel> SaveAsync(OpenOfficeHoursMeetingsStatusDataModel model);
    }
}
