using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization
{
   public interface ISignUpService
    {
        Task<BaseModel> SignUpAsync(SignUpViewModel model);
    }
}
