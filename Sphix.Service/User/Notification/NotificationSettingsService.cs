using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Context;
using Sphix.DataModels.User;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;

namespace Sphix.Service.User.Notification
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private UnitOfWork _unitOfWork;
        public NotificationSettingsService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> SaveAsync(NotificationSettingsViewModel model)
        {
            var userData = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
            if (userData== null)
            {
                userData = null;
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
            var result = await _unitOfWork.UserNotificationSettingsRepository.FindAllBy(c=>c.User.Id==model.UserId);
            var notificationData = result.FirstOrDefault();
            if (notificationData==null)
            {
                //insert
               await _unitOfWork.UserNotificationSettingsRepository.Insert(new UserNotificationSettingsDataModel
                {
                   BlogSubscription=model.BlogSubscription,
                   Comments=model.Comments,
                   Followis=model.Followis,
                   Like=model.Like,
                   UpdatedBy=model.UserId,
                   CreatedBy = model.UserId,
                   AddedDate=DateTime.Now,
                   User=userData
               });
            }
            else
            {
                notificationData.BlogSubscription = model.BlogSubscription;
                notificationData.Comments = model.Comments;
                notificationData.Followis = model.Followis;
                notificationData.Like =model.Like;
                notificationData.UpdatedBy = model.UserId;
                //update 
                await _unitOfWork.UserNotificationSettingsRepository.Update(notificationData);
            }
            return new BaseModel { Status = true, Messsage = UMessagesInfo.RecordSaved };

        }
        public async Task<NotificationSettingsViewModel> GetNotificationSettingsAsync(long UserId)
        {
            var result= await _unitOfWork.UserNotificationSettingsRepository.FindAllBy(c => c.User.Id == UserId);
            if (result.Count != 0)
            {
                return new NotificationSettingsViewModel{
                    BlogSubscription=result[0].BlogSubscription,
                    Comments = result[0].Comments,
                    Followis = result[0].Followis,
                    Like = result[0].Like
                };
            }
            return new NotificationSettingsViewModel();
        }
    }
}
