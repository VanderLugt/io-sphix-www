using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization.Login.ForgotPassword
{
   public interface IForgotPasswordService
    {
        Task<BaseModel> SendForgotPasswordLinkAsync(string UserName, string HtmlBody, string verificationActionPath);
        Task<UserShortProfileViewModel> ValidateTokenAsync(string token);
    }
}
