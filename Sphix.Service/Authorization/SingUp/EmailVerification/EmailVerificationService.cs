using Data.Context;
using Microsoft.EntityFrameworkCore;
using Sphix.DataModels.Authorization;
using Sphix.DataModels.User;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.Service.User;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.Utility.DateTimeDifference;
using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization.SignUp.EmailVerification
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private UnitOfWork _unitOfWork;
        private readonly IEmailSenderService _emailSender;
        private readonly IUDateTimeDifference _uDateTimeDifference;
        private readonly IUserService _userService;
        private SphixConfiguration _sphixConfiguration { get; }
        public EmailVerificationService(EFDbContext context, IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration, IUDateTimeDifference uDateTimeDifference
            , IUserService userService)
        {
            _unitOfWork = new UnitOfWork(context);
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
            _uDateTimeDifference = uDateTimeDifference;
            _userService = userService;
        }
        public async Task<BaseModel> SendEmailVerification(long Id, string HtmlBody, string verificationActionPath,string Name)
        {
            bool _actionStatus = false;
            UsersLoginDataModel usersLoginData = await _unitOfWork.UserLoginRepository.GetByID(Id);
            if (usersLoginData == null)
            {
                return new BaseModel { Status = false, Messsage = UMessagesInfo.WrongUserName };
            }
            string token = Guid.NewGuid().ToString().Replace("-","");

            var _resultEmailVerification =await  _unitOfWork.EmailVerificatioRepository.FindAllBy(c => c.User.Id == Id);

            if (_resultEmailVerification.Count!=0)
            {
                var emailVerificationUpdateModel = _resultEmailVerification.FirstOrDefault();
                emailVerificationUpdateModel.VerificationCode = token;
                emailVerificationUpdateModel.IsVerified = false;
                await _unitOfWork.EmailVerificatioRepository.Update(emailVerificationUpdateModel);
                _actionStatus = true;
            }
            else
            {
                EmailVerificationDataModel emailVerificationModel = new EmailVerificationDataModel
                {
                    User = usersLoginData,
                    EmailAddress = usersLoginData.UserName,
                    IsVerified = false,
                    VerificationCode = token
                };
               await _unitOfWork.EmailVerificatioRepository.Insert(emailVerificationModel);
                _actionStatus = true;
            }

            if (_actionStatus)
            {
                //active user profile on email verification
                usersLoginData.IsActive = true;
                usersLoginData.UpdatedBy = Id;
                usersLoginData.CreatedBy = Id;
                await _unitOfWork.UserLoginRepository.Update(usersLoginData);
                //set user information in verification email message 
                string messageBody = string.Format(HtmlBody,
                        verificationActionPath + token,
                        Name,
                        usersLoginData.UserName
                        );

                await _emailSender.SendEmailAsync(
                     UMessagesInfo.VerificationEmailSubject,
                     messageBody,
                     usersLoginData.UserName,
                     _sphixConfiguration.SupportEmail,
                     UMessagesInfo.SphixSupport
                     );
                return new BaseModel { Status = true, Messsage = UMessagesInfo.SignUpSuccess };
            }
            return new BaseModel { Status = true, Messsage = UMessagesInfo.Error };
        }

        public async Task<UserShortProfileViewModel> ValidateToken(string token)
        {
            IQueryable<EmailVerificationDataModel> _query = _unitOfWork.EmailVerificatioRepository.FindAllByQuery(c => c.VerificationCode == token);
            IList<EmailVerificationDataModel> _result = await _query.Include("User").ToListAsync();
            _query = null;
            EmailVerificationDataModel item = _result.FirstOrDefault();
            _result = null;
            if (item!=null)
            {
                
                if (!item.IsVerified)
                {
                    if (_uDateTimeDifference.CheckTimeIsExpiredDays(DateTime.Now, item.AddedDate, 24))
                    {
                        return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.TokenExpired };
                    }
                    else
                    {
                        //update verification in database
                        item.IsVerified = true;
                        item.VerificationDate = DateTime.UtcNow;
                        await _unitOfWork.EmailVerificatioRepository.Update(item);
                        return await _userService.GetUserShortProfileById(item.User.Id);
                    }
                }
                else
                {
                    return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.TokenAlreadyUsed };
                }
            }
            return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.TokenNotExists };
        }
    }
}
