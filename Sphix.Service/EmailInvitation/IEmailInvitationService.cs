using Sphix.ViewModels;
using Sphix.ViewModels.EmailInvitation;
using System.Threading.Tasks;

namespace Sphix.Service.EmailInvitation
{
   public interface IEmailInvitationService
    {
        Task<BaseModel> sendGroupEmailInvitation(GroupEmailInvitationViewModel model, string htmlBody, string callbackUrl);
        Task<BaseModel> checkGroupInvitationToken(string token, long UserId);

    }
}
