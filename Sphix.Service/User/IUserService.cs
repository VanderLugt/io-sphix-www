
using Microsoft.AspNetCore.Http;
using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphix.Service.User
{
   public interface IUserService
    {
        Task<UserProfileViewModel> UpdateProfileAsync(UserProfileViewModel model);
        Task<UserShortProfileViewModel> UpdateProfilePictureAsync(IFormFile file,long UserId);
        Task<UserProfileViewModel> GetUserProfileByIdAsync(long UserId);
        Task<UserShortProfileViewModel> GetUserShortProfileById(long Id);
        Task<BaseModel> ChangePasswordAsync(ResetPaswordViewModel model);
        Task<IList<AdminUsersList>> GetAdminUsersListAsync(CustomeSearchFilter model);
        Task<BaseModel> UpdateUserRoleAsync(long RoleId, long UserId);


    }
}
