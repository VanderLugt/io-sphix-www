using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Threading.Tasks;
namespace Sphix.Service.Authorization.Login
{
   public interface ILoginService
    {
        Task<UserClaimsViewModel> LoginAsync(LoginViewModel model);
    }
}
