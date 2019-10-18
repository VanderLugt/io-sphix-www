using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Threading.Tasks;

namespace Sphix.Service.User.Notification
{
   public interface INotificationSettingsService
    {
        Task<BaseModel> SaveAsync(NotificationSettingsViewModel model);
        Task<NotificationSettingsViewModel> GetNotificationSettingsAsync(long UserId);
    }
}
