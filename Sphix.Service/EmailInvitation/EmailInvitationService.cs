using System;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Sphix.DataModels.EmailInvitation;
using Sphix.Service.SendGridManager;
using Sphix.Service.Settings;
using Sphix.Service.UserCommunities.JoinCommunityGroup;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.EmailInvitation;
using Sphix.ViewModels.UserCommunities;

namespace Sphix.Service.EmailInvitation
{
    public class EmailInvitationService : IEmailInvitationService
    {
        private UnitOfWork _unitOfWork;
        private SphixConfiguration _sphixConfiguration { get; }
        private readonly IEmailSenderService _emailSender;
        private readonly IJoinCommunityGroupService _joinCommunityGroup;
        public EmailInvitationService(EFDbContext context
            , IEmailSenderService emailSender
            , SphixConfiguration sphixConfiguration
            , IJoinCommunityGroupService joinCommunityGroup
            )
        {
            _emailSender = emailSender;
            _sphixConfiguration = sphixConfiguration;
            _joinCommunityGroup = joinCommunityGroup;
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> sendGroupEmailInvitation(GroupEmailInvitationViewModel model,string htmlBody,string callbackUrl)
        {
            var groupMoodel = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.CommunityGroup);
            //var existingInvitation = await _unitOfWork.GroupEmailInvitationRepository.FindAllBy(c => c.ToEmailAddress == model.ToEmailAddress && c.CommunityGroup.Id == model.CommunityGroup);
            //var userProfile = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == model.SentByUser);
            GroupEmailInvitationDataModel uModel = new GroupEmailInvitationDataModel() {
                 IsAccpeted=false,
                 CommunityGroup= groupMoodel,
                 LastUpdate= DateTime.Now,
                 ReSend=0,
                 SentByUser=await _unitOfWork.UserLoginRepository.GetByID(model.SentByUser),
                 SentOn=DateTime.Now,
                 SentTo=model.SentTo,
                 ToEmailAddress=model.ToEmailAddress
            };
            await _unitOfWork.GroupEmailInvitationRepository.Insert(uModel);
            sendEmail(htmlBody, model.ToEmailAddress, groupMoodel.Title, callbackUrl+uModel.Id);
            return new BaseModel { Status = false, Messsage = UMessagesInfo.RecordSaved };
        }
        public async Task<BaseModel> checkGroupInvitationToken(string token,long UserId)
        {
            var query =  _unitOfWork.GroupEmailInvitationRepository.FindAllByQuery(c=>c.Id== new Guid(token));
            var result = await query.Include("CommunityGroup").ToListAsync();
            GroupEmailInvitationDataModel model = new GroupEmailInvitationDataModel();
            if (result != null)
            {
                model = result[0];
                if (model.IsAccpeted)
                {
                    return new BaseModel { Status = true, Messsage = model.CommunityGroup.CommunityGroupURL + "-" + model.CommunityGroup.Id.ToString() };
                }
                JoinCommunityGroupViewModel joinCommunityModel = new JoinCommunityGroupViewModel {
                    CommunityGroupId=model.CommunityGroup.Id,
                    UserId=UserId
                };
                await _joinCommunityGroup.JoinCommunityGroupAsync(joinCommunityModel);
                model.IsAccpeted = true;
                model.LastUpdate = DateTime.Now;
                model.SentTo = UserId;
                model.AcceptedOn = DateTime.Now;

             await  _unitOfWork.GroupEmailInvitationRepository.Update(model);
            }
            return new BaseModel { Status = true, Messsage = model.CommunityGroup.CommunityGroupURL + "-" + model.CommunityGroup.Id.ToString() };

        }
        private void sendEmail(string htmlBody, string emailTo,string groupName,string groupUrl)
        {
            string messageBody = string.Format(htmlBody,
                           groupName,
                           groupUrl,
                           groupName,
                           UMessagesInfo.MailFooter
                           );

             _emailSender.SendEmailAsync(
                 UMessagesInfo.GroupEmailInvitationSubject,
                 messageBody,
                 emailTo,
                 _sphixConfiguration.SupportEmail,
                 UMessagesInfo.SphixSupport
                 );
            
        }

       
    }
}
