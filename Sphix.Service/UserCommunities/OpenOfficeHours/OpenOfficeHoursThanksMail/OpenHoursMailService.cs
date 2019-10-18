using Data.Context;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursThanksMail
{
    public class OpenHoursMailService:IOpenHoursMailService
    {
        private UnitOfWork _unitOfWork;
        private readonly IEmailSenderService _emailSender;
        private SphixConfiguration _sphixConfiguration { get; }
        public OpenHoursMailService(EFDbContext context, IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration)
        {
            _unitOfWork = new UnitOfWork(context);
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
        }
        public async Task<BaseModel> SendMailOnCreateNewTableAsync(long userId, string token,string callBackLink, string emailTemplateBody,string meetingDateAndTime)
        {
            try
            {
                var _data = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == userId);
                if (_data != null)
                {
                    if (string.IsNullOrEmpty(_data[0].Email))
                    {
                        return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
                    }
                    string messageBody = string.Format(emailTemplateBody,
                            _data[0].FirstName,
                            meetingDateAndTime,
                             callBackLink,
                            UMessagesInfo.MailFooter
                            );
                    await _emailSender.SendEmailAsync(
                            "Open Office Hours Meeting",
                            messageBody,
                            _data[0].Email,
                            _sphixConfiguration.SupportEmail,
                            UMessagesInfo.SphixSupport
                            );
                    return new BaseModel { Status = true, Messsage = UMessagesInfo.EmailSent };
                }
                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
            catch (Exception)
            {

                return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
            }
        }
    }
}
