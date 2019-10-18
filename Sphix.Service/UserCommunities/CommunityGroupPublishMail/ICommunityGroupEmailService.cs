using Sphix.ViewModels;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.CommunityGroupPublishMail
{
   public interface ICommunityGroupEmailService
    {
        Task<BaseModel> SendJoinCommunityEmailAsync(long userId, long communityId, string emailTemplateBody);
        Task<BaseModel> SendCommunityGroupPublishEmailAsync(long Id, string EmailTemplateBody, string callbackUrl);
        Task<BaseModel> CommunityGroupVerificationAsync(string token);
    }
}
