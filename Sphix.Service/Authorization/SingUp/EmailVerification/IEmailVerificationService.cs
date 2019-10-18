using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization.SignUp.EmailVerification
{
   public interface IEmailVerificationService
    {
        Task<BaseModel> SendEmailVerification(long Id, string HtmlBody, string verificationActionPath, string Name);
        Task<UserShortProfileViewModel> ValidateToken(string token);
    }
}
