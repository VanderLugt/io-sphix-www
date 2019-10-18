using Data.Context;
using Sphix.DataModels.UserCommunitiesGroups.Join;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System;
using System.Threading.Tasks;

namespace Sphix.Service.UserCommunities.JoinCommunityGroup
{
   public class JoinCommunityGroupService:IJoinCommunityGroupService
    {
        private UnitOfWork _unitOfWork;
        public JoinCommunityGroupService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<BaseModel> JoinCommunityGroupAsync(JoinCommunityGroupViewModel model)
        {
            JoinCommunityGroupDataModel joinCommunityGroupData = new JoinCommunityGroupDataModel();
            if(model.Id==0)
            {
                var _result =await _unitOfWork.JoinCommunityGroupRepository.FindAllBy(c => c.User.Id == model.UserId && c.CommunityGroup.Id==model.CommunityGroupId);
                if (_result.Count == 0)
                {
                    //insert record
                    joinCommunityGroupData.CommunityGroup = await _unitOfWork.UserCommunityGroupsRepository.GetByID(model.CommunityGroupId);
                    joinCommunityGroupData.User = await _unitOfWork.UserLoginRepository.GetByID(model.UserId);
                    joinCommunityGroupData.IsJoined = true;
                    joinCommunityGroupData.JoinDateTime = DateTime.Now;
                    await _unitOfWork.JoinCommunityGroupRepository.Insert(joinCommunityGroupData);
                }
                else
                {
                    //user want to re-join community group
                    _result[0].IsJoined = true;
                    await _unitOfWork.JoinCommunityGroupRepository.Update(_result[0]);
                }
            }
            else
            {
                //update record
                joinCommunityGroupData = await _unitOfWork.JoinCommunityGroupRepository.GetByID(model.Id);
                joinCommunityGroupData.IsJoined = model.IsJoin;
                await _unitOfWork.JoinCommunityGroupRepository.Update(joinCommunityGroupData);
            }
            return new BaseModel {Id=joinCommunityGroupData.Id,Status=true,Messsage= UMessagesInfo.CommunityGroupJoinedSuccessfully };
        }
       
    }
}
