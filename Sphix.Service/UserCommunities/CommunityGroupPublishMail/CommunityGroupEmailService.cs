using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups.PublishCommunityGroupLink;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.CommunityGroupPublishMail
{
   public class CommunityGroupEmailService : ICommunityGroupEmailService
    {
        private UnitOfWork _unitOfWork;
        private readonly IEmailSenderService _emailSender;
        private SphixConfiguration _sphixConfiguration { get; }
        public CommunityGroupEmailService(EFDbContext context, IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration)
        {
            _unitOfWork = new UnitOfWork(context);
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
        }
        public async Task<BaseModel> SendCommunityGroupPublishEmailAsync(long Id,string EmailTemplateBody,string callbackUrl)
        {
            try
            {
                var _data = await _unitOfWork.UserCommunityGroupsRepository.GetByID(Id);
                if (_data != null)
                {
                    string token = Guid.NewGuid().ToString().Replace("-", "");
                    UserCommunityGroupPublishLinksDataModel model = new UserCommunityGroupPublishLinksDataModel()
                    {
                        CommunityGroupId = Id,
                        VerificationCode = token,
                        CreatedDate = DateTime.UtcNow,
                        IsPublished = false
                    };
                    await _unitOfWork.CommunityGroupPublishLinksRepository.Insert(model);

                    string messageBody = string.Format(EmailTemplateBody,
                            _data.Title,
                             UMessagesInfo.AWSPublicURL + _data.DescriptionVideoUrl,
                            _data.Description,
                            callbackUrl + token
                            );
                    await _emailSender.SendEmailAsync(
                            UMessagesInfo.NewCommunityGroupEmailSubject,
                            messageBody,
                            _sphixConfiguration.SuperAdminEmail,
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
        public async Task<BaseModel> SendJoinCommunityEmailAsync(long userId,long communityId, string emailTemplateBody)
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
                    var _communtyDetail = await _unitOfWork.CommunityRepository.GetByID(communityId);
                    string messageBody = string.Format(emailTemplateBody,
                            _data[0].FirstName ,
                            _communtyDetail.Name,
                            _communtyDetail.Description,
                            UMessagesInfo.MailFooter
                            );
                    await _emailSender.SendEmailAsync(
                            UMessagesInfo.JoinCommunityEmailSubject,
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
        public async Task<BaseModel> CommunityGroupVerificationAsync(string token)
        {
            try
            {
                var _result =await _unitOfWork.CommunityGroupPublishLinksRepository.FindAllBy(c => c.VerificationCode == token);
                if(_result!=null && _result.Count>0)
                {
                    var _data = _result[0];
                    var communtyGroupModel =await _unitOfWork.UserCommunityGroupsRepository.GetByID(_data.CommunityGroupId);
                    communtyGroupModel.IsPublish = true;
                    await _unitOfWork.UserCommunityGroupsRepository.Update(communtyGroupModel);

                    //update status in token verfication table
                    _data.IsPublished = true;
                    await _unitOfWork.CommunityGroupPublishLinksRepository.Update(_data);
                    return new BaseModel {Status=true };

                }
                return new BaseModel { Status = false, Messsage = UMessagesInfo.RecordNotExist };
            }
            catch (Exception)
            {
                return new BaseModel { Status = false,Messsage= UMessagesInfo.Error };
                
            }
        }
    }
}
