using Data.Context;
using Sphix.DataModels.Communities;
using Sphix.DataModels.User;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.User.UserCommunities
{
    public class UserCommunitiesService : IUserCommunitiesService
    {
        private UnitOfWork _unitOfWork;

        public UserCommunitiesService(EFDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public async Task<bool> SaveCommunitiesAsync(string communitiesIds, UsersLoginDataModel model)
        {
            string[] communities = communitiesIds.Split(',');
            foreach (var item in communities)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    UserCommunitiesDataModel userCommunities = new UserCommunitiesDataModel();
                    userCommunities.User = model;
                    userCommunities.CommunityId = Convert.ToInt64(item);
                    userCommunities.IsActive = true;

                    await _unitOfWork.UserCommunitiesRepository.Insert(userCommunities);
                }
                //Console.WriteLine(item);
            }
            communities = null;
            return true;
        }
        public async Task<BaseModel> JoinCommunitiesAsync(long communitieId, long UserId)
        {
            try
            {
                if (communitieId == 0 || UserId==0)
                {
                    return new BaseModel { Status = false, Messsage = UMessagesInfo.Error };
                }
                var _result = await _unitOfWork.UserCommunitiesRepository.FindAllBy(c => c.CommunityId == communitieId && c.User.Id == UserId);
                if (_result.Count == 0)
                {
                    UserCommunitiesDataModel userCommunities = new UserCommunitiesDataModel();
                    userCommunities.User = await _unitOfWork.UserLoginRepository.GetByID(UserId);
                    userCommunities.CommunityId = communitieId;
                    userCommunities.IsActive = true;
                    await _unitOfWork.UserCommunitiesRepository.Insert(userCommunities);
                }
                else
                {
                    var _data = _result[0];
                    _data.IsActive = true;
                    await _unitOfWork.UserCommunitiesRepository.Update(_data);
                }
                return new BaseModel { Status = true, Messsage = UMessagesInfo.CommunityGroupJoinedSuccessfully };
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> SaveSubCommunitiesAsync(UserSubCommunitiesDataModel model)
        {
            if (model.Id == 0)
            {
                await _unitOfWork.UserSubCommunitiesRepository.Insert(model);
            }
            else
            {
                var _data = await _unitOfWork.UserSubCommunitiesRepository.GetByID(model.Id);
                //update only same user data
                if (_data.User == model.User)
                {
                    _data.Type1Id = model.Type1Id;
                    _data.Type2ListId = model.Type2ListId;
                    _data.GroupId = model.GroupId;
                    _data.Association = model.Association;
                    await _unitOfWork.UserSubCommunitiesRepository.Update(_data);
                }
            }
            
            return true;
        }
        public async Task<bool> SaveInterestsAsync(string interestsIds, UsersLoginDataModel user, CommunityDataModel community)
        {
            if (interestsIds == null)
            {
                return false;
            }
            string[] interests = interestsIds.Split(',');
            var _selectedInterests =await _unitOfWork.UserInterestsRepository.FindAllBy(c => c.User == user && c.Community == community);

            foreach (var updateItem in _selectedInterests)
            {
                if (interests.Contains(updateItem.InterestId.ToString()) == false)
                {
                    updateItem.IsActive = false;
                }
                else
                {
                    updateItem.IsActive = true;
                    interests = interests.Where(val => val != updateItem.InterestId.ToString()).ToArray();
                }
                await _unitOfWork.UserInterestsRepository.Update(updateItem);

            }
            foreach (var item in interests)
            {
                if (!string.IsNullOrEmpty(item) && item!="0")
                {
                   
                        UserInterestsDataModel userInterests = new UserInterestsDataModel();
                        userInterests.User = user;
                        userInterests.Community = community;
                        userInterests.IsActive = true;
                        userInterests.InterestId = Convert.ToInt32(item);
                        await _unitOfWork.UserInterestsRepository.Insert(userInterests);
                   
                }
                //Console.WriteLine(item);
            }
            
            return true;
        }
    }
}
