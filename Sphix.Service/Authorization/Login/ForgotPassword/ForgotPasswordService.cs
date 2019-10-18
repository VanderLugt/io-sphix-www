using Data.Context;
using Sphix.DataModels.Authorization;
using Sphix.DataModels.User;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Microsoft.EntityFrameworkCore;
using Sphix.ViewModels.User;
using Sphix.Service.User;

namespace Sphix.Service.Authorization.Login.ForgotPassword
{
    public class ForgotPasswordService : IForgotPasswordService
    {
        private UnitOfWork _unitOfWork;
        private readonly IEmailSenderService _emailSender;
        private  SphixConfiguration _sphixConfiguration { get; }
        private readonly IUserService _userService;
        public ForgotPasswordService(EFDbContext context, IEmailSenderService emailSender
            ,SphixConfiguration sphixConfiguration, IUserService userService)
        {
            _unitOfWork = new UnitOfWork(context);
            _sphixConfiguration = sphixConfiguration;
            _emailSender = emailSender;
            _userService = userService;
        }
        public async Task<BaseModel> SendForgotPasswordLinkAsync(string UserName, string HtmlBody, string verificationActionPath)
        {
            IList<UsersLoginDataModel> users = await _unitOfWork.UserLoginRepository.FindAllBy(c => c.UserName.ToLower() == UserName);
            UsersLoginDataModel userData = users.FirstOrDefault();
            if (userData != null)
            {
                string token = Guid.NewGuid().ToString().Replace("-", "");
                RestPasswordLinkDataModel resetPasswordModel = new RestPasswordLinkDataModel {
                    IsRequestHasUsed=false,
                    IsRequestToRestPassword=true,
                    User=userData,
                    VerificationCode = token
                };
                var _data= await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == userData.Id);
                if (_data != null)
                {
                    var resetPasswordExistingDetail = await _unitOfWork.RestPasswordLinkRepository.FindAllBy(c => c.User == userData);
                    RestPasswordLinkDataModel resetPasswordUpdateModel = resetPasswordExistingDetail.FirstOrDefault();
                    if (resetPasswordUpdateModel == null)
                    {
                        await _unitOfWork.RestPasswordLinkRepository.Insert(resetPasswordModel);
                    }
                    else
                    {
                        resetPasswordUpdateModel.IsRequestToRestPassword = true;
                        resetPasswordUpdateModel.IsRequestHasUsed = false;
                        resetPasswordUpdateModel.VerificationCode = token;
                        await _unitOfWork.RestPasswordLinkRepository.Update(resetPasswordUpdateModel);
                    }

                    string messageBody = string.Format(HtmlBody,
                           verificationActionPath + token,
                           _data.FirstOrDefault().FirstName + ' ' + _data.FirstOrDefault().LastName,
                           userData.UserName
                           );

                    await _emailSender.SendEmailAsync(
                               UMessagesInfo.ResetPasswordSubject,
                               messageBody,
                               userData.UserName,
                               _sphixConfiguration.SupportEmail,
                               UMessagesInfo.SphixSupport
                               );

                    return new BaseModel { Status = true, Messsage = UMessagesInfo.ResetPasswordLinkSentOnMail };
                }
                
              return new BaseModel { Status = false, Messsage = UMessagesInfo.UserNameNotExist };
            }
            return new BaseModel {Status=false,Messsage=UMessagesInfo.UserNameNotExist };
        }
        public async Task<UserShortProfileViewModel> ValidateTokenAsync(string token)
        {
            IQueryable<RestPasswordLinkDataModel> _query = _unitOfWork.RestPasswordLinkRepository.FindAllByQuery(c => c.VerificationCode == token);
            IList<RestPasswordLinkDataModel> _result = await _query.Include("User").ToListAsync();
            RestPasswordLinkDataModel _forgotPasswordDetail = _result.FirstOrDefault();
            _result = null;
            if (_forgotPasswordDetail != null)
            {

                if (_forgotPasswordDetail.IsRequestToRestPassword && _forgotPasswordDetail.IsRequestHasUsed==false)
                {
                    //_forgotPasswordDetail.IsRequestHasUsed = true;
                    //_forgotPasswordDetail.IsRequestToRestPassword = false;
                    //_forgotPasswordDetail.VerificationDate = DateTime.Now;
                    // await _unitOfWork.RestPasswordLinkRepository.Update(_forgotPasswordDetail);
                    return await _userService.GetUserShortProfileById(_forgotPasswordDetail.User.Id);
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
