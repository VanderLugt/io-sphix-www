using Sphix.ViewModels;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursThanksMail
{
   public interface IOpenHoursMailService
    {
        Task<BaseModel> SendMailOnCreateNewTableAsync(long userId, string token, string callBackLink, string emailTemplateBody, string meetingDateAndTime);
    }
}
