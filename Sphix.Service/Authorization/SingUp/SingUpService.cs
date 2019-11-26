using Data.Context;
using Microsoft.Extensions.Options;
using Sphix.DataModels.User;
using Sphix.Service.User.UserCommunities;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization
{
   public class SignUpService: ISignUpService
    {
        private UnitOfWork _unitOfWork;
        private readonly IUserCommunitiesService _userCommunities;
        private readonly PasswordSettings _settings;
        public SignUpService(EFDbContext context, IUserCommunitiesService userCommunities, IOptions<PasswordSettings> settings)
        {
            _unitOfWork = new UnitOfWork(context);
            _userCommunities = userCommunities;
            _settings = settings.Value;
        }
        public async Task<BaseModel> SignUpAsync(SignUpViewModel model)
        {
            var roleData = await _unitOfWork.RoleRepository.FindAllBy(c => c.RoleName.ToLower() == "user");
            if(roleData.Count==0)
            {
                return new BaseModel { Status = false, Messsage = "no data for user role in role master table" };
            }
            model.Email = model.Email.ToLower().Trim();
            var existingUser = await _unitOfWork.UserLoginRepository.FindAllBy(c => c.UserName == model.Email);
            if (existingUser.Count>0)
            {
                existingUser = null;
                return new BaseModel { Status = false, Messsage = String.Format(UMessagesInfo.UserNameExist, model.Email) };
            }
            else
            {
                string saltValue = Guid.NewGuid().ToString();
                string password = UPasswordRijndaelAlgorithm.Encrypt(model.Password, saltValue,_settings.PassPhrase,_settings.HashAlgorithm,_settings.InitVector);
                UsersLoginDataModel userLoginData = new UsersLoginDataModel()
                {
                    Password = password,
                    UserName = model.Email,
                    PasswordSalt=saltValue,
                    AddedDate=DateTime.Now,
                    IsActive=false
                };
                var userData = await _unitOfWork.UserLoginRepository.Insert(userLoginData);
                
                await _unitOfWork.UsersRolesRepository.Insert(new UsersRolesDataModel {User=userData,Role= roleData.FirstOrDefault() });
                //string your_String = "kyle.VanderLugt@gmail.com(Hello)";
                string profileLink = Regex.Replace((model.FirstName+ model.LastName), @"[^0-9a-zA-Z]+", "").Replace(" ","");
                var profilelinkCheck =await _unitOfWork.UserProfileRepository.FindAllBy(c => c.ProfileLink == profileLink);
                if(profilelinkCheck.Count!=0)
                {
                    profilelinkCheck = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.ProfileLink == profileLink+"S00"+userData.Id.ToString());
                    if (profilelinkCheck.Count != 0)
                    {
                        profileLink = Regex.Replace(Guid.NewGuid().ToString(), @"[^0-9a-zA-Z]+", "");
                    }
                    else
                    {
                        profileLink = profileLink + "S0" + userData.Id.ToString();
                    }
                   
                }
                UsersProfileDataModel usersProfileData = new UsersProfileDataModel
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.PhoneNumber,
                    UseMyLocation = model.UseMyLocation,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    User =  userData,
                    IsCertifyTAndC=model.IsCertifyTAndC,
                    IsFinanciallyActive=model.IsFinanciallyActive,
                    IsKickstartarActive=model.IsKickstartarActive,
                    AddedDate = DateTime.UtcNow,
                    ProfileLink=profileLink
                };
                var profileData = await _unitOfWork.UserProfileRepository.Insert(usersProfileData);
                Int64 UserId = usersProfileData.User.Id;

                profileData.UpdatedBy = UserId;
                profileData.CreatedBy = UserId;
                await _unitOfWork.UserProfileRepository.Update(profileData);
                profileData = null;
                await _userCommunities.SaveCommunitiesAsync(model.CommunityId,  userData);
                var _mainCommunity = await _unitOfWork.CommunityRepository.GetByID(Convert.ToInt32(model.CommunityId));
                if (_mainCommunity != null)
                {
                    await _userCommunities.SaveGroupsAsync(model.GroupId.ToString(), userData, _mainCommunity);
                    await _userCommunities.SaveAssociationsAsync(model.AssociationId.ToString(), userData, _mainCommunity);

                    //Save data into the Interests table 
                    await _userCommunities.SaveInterestsAsync(model.InterestsId, userData, _mainCommunity);
                    //Save data into the community table like group,association 
                    //UserSubCommunitiesDataModel userCommunities = new UserSubCommunitiesDataModel
                    //{
                    //    User = userData,
                    //    Association = model.AssociationId,
                    //    GroupId = model.GroupId,
                    //    Type1Id = model.Type1ListId,
                    //    Type2ListId = model.Type2ListId,
                    //    Community = _mainCommunity

                    //};
                    //await _userCommunities.SaveSubCommunitiesAsync(userCommunities);
                    //userCommunities = null;
                }

               
                userLoginData = null;
                usersProfileData = null;
                _mainCommunity = null;
                userData = null;
                return new BaseModel { Id = UserId, Status = true, Messsage = UMessagesInfo.SignUpSuccess };
            }
        }
    }
}
