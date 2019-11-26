using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AWSS3.Utility;
using Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sphix.DataModels.Authorization;
using Sphix.DataModels.User;
using Sphix.Service.Authorization;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;

namespace Sphix.Service.User
{
    public class UserService : IUserService
    {
        private UnitOfWork _unitOfWork;
        private readonly PasswordSettings _settings;
        private readonly IAWSS3Bucket _awsS3Bucket;
        public UserService(EFDbContext context, IOptions<PasswordSettings> settings, IAWSS3Bucket awsS3Bucket)
        {
            _unitOfWork = new UnitOfWork(context);
            _settings = settings.Value;
            _awsS3Bucket = awsS3Bucket;
        }
        public async Task<UserProfileViewModel> UpdateProfileAsync(UserProfileViewModel model)
        {
            var _result = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.Id == model.Id && c.User.Id == model.LogedInUserId);

            UsersProfileDataModel userProfile = _result.FirstOrDefault();
          //  model.ProfileLink = profileLink;
            userProfile.FirstName = model.FirstName;
            userProfile.LastName = model.LastName;
            userProfile.Email = model.EmailAddress;
            userProfile.Phone = model.PhoneNumber;
            //userProfile.ProfileLink = model.ProfileLink;
            userProfile.UpdatedBy = model.LogedInUserId;
            userProfile.ModifiedDate = DateTime.UtcNow;
            //update user detail
            await _unitOfWork.UserProfileRepository.Update(userProfile);

            userProfile = null;
            model.Status = true;
            model.Messsage = UMessagesInfo.RecordSaved;
            return model;
          
        }
        public async Task<UserShortProfileViewModel> UpdateProfilePictureAsync(IFormFile file, long UserId)
        {
            string folderPath = "ProfilePicture/" + DateTime.Now.Year.ToString();
            string fileName = Guid.NewGuid().ToString().Replace("-", "")+ Path.GetExtension(file.FileName);
            if (await _awsS3Bucket.UploadFileAsync(folderPath, file, fileName))
            {
                var _result = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == UserId);
               
                if(_result.Count>0)
                {
                    var _userData = _result.FirstOrDefault();
                    //remove old file from AWS S3 bucket
                    await _awsS3Bucket.DeleteFileAsync(_userData.ProfilePicture);

                    _userData.ProfilePicture = folderPath+"/"+ fileName;
                   await _unitOfWork.UserProfileRepository.Update(_userData);
                }
            }
           return new UserShortProfileViewModel {Status=true,ProfilePicture= folderPath + "/" + fileName };
        }
        public async Task<UserProfileViewModel> GetUserProfileByIdAsync(long UserId)
        {
            bool IsVerified = false;
            IQueryable <UsersProfileDataModel> _query =  _unitOfWork.UserProfileRepository.FindAllByQuery(c => c.User.Id == UserId);
            IList<UsersProfileDataModel> _result = await _query.Include("User").ToListAsync();
            
            if (_result.Count != 0)
            {
                var _emails = await _unitOfWork.EmailVerificatioRepository.FindAllBy(c => c.User == _result[0].User);
                if(_emails.Count==0)
                {
                    IsVerified = false;
                }
                else
                {
                    IsVerified = _emails[0].IsVerified;
                }
                UserProfileViewModel userProfile = new UserProfileViewModel()
                {   Id=_result[0].Id,
                    UserName = _result[0].User.UserName,
                    FirstName = _result[0].FirstName,
                    LastName = _result[0].LastName,
                    ProfileLink= _result[0].ProfileLink,
                    EmailAddress=_result[0].Email,
                    PhoneNumber=_result[0].Phone,
                    IsVerified = IsVerified,
                    Status = true
                };
                _result = null;
                return userProfile;
            }
            return new UserProfileViewModel { Status = false, Messsage = UMessagesInfo.RecordNotExist };
        }
        public async Task<UserShortProfileViewModel> GetUserShortProfileById(long UserId)
        {
            var _data = await _unitOfWork.UserProfileRepository.FindAllBy(c => c.User.Id == UserId);
            var _query = _unitOfWork.UsersRolesRepository.FindAllByQuery(c => c.User.Id == UserId);
            var _roles = await _query.Include("Role").ToListAsync();
            if (_data.Count != 0)
            {
                UserShortProfileViewModel userShortProfile = new UserShortProfileViewModel()
                {
                    Email = _data[0].Email,
                    FirstName = _data[0].FirstName,
                    LastName = _data[0].LastName,
                    Status = true,
                    ProfilePicture=_data[0].ProfilePicture,
                    Roles=_roles[0].Role.RoleName
                };
                var _communityGroups = await _unitOfWork.CommunityRepository.FindAllBy(c => c.IsActive);
                List<CommunityGroups> communityGroups = new List<CommunityGroups>();
                foreach (var item in _communityGroups.OrderBy(c=>c.DisplayIndex))
                {
                    communityGroups.Add(new CommunityGroups
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Color=item.Color,
                        CommunityUrl=item.CommunityUrl
                    });
                }
                _data = null;
                userShortProfile.communityGroups = communityGroups;
                return userShortProfile;
            }
            return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.RecordNotExist };
        }
        public async Task<BaseModel> ChangePasswordAsync(ResetPaswordViewModel model)
        {
            IQueryable<RestPasswordLinkDataModel> _query = _unitOfWork.RestPasswordLinkRepository.FindAllByQuery(c => c.VerificationCode == model.Token);
            IList<RestPasswordLinkDataModel> _result = await _query.Include("User").ToListAsync();
            RestPasswordLinkDataModel _forgotPasswordDetail = _result.FirstOrDefault();
            _result = null;
            if (_forgotPasswordDetail != null)
            {

                if (_forgotPasswordDetail.IsRequestToRestPassword && _forgotPasswordDetail.IsRequestHasUsed == false)
                {
                    //reset password functionality
                    string password = UPasswordRijndaelAlgorithm.Encrypt(model.Password, _forgotPasswordDetail.User.PasswordSalt, _settings.PassPhrase, _settings.HashAlgorithm, _settings.InitVector);
                    _forgotPasswordDetail.User.Password = password;
                   if( await UpdateUser(_forgotPasswordDetail.User))
                    {
                        _forgotPasswordDetail.IsRequestHasUsed = true;
                        _forgotPasswordDetail.IsRequestToRestPassword = false;
                        _forgotPasswordDetail.VerificationDate = DateTime.UtcNow;
                        await _unitOfWork.RestPasswordLinkRepository.Update(_forgotPasswordDetail);
                        return new UserShortProfileViewModel { Status = true, Messsage = UMessagesInfo.ResetPasswordSuccessfully };
                    }
                   else
                    {
                        return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.Error };
                    }

                }
                else
                {
                    return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.TokenAlreadyUsed };
                }
            }
            return new UserShortProfileViewModel { Status = false, Messsage = UMessagesInfo.TokenNotExists };
        }
        private async Task<bool> UpdateUser(UsersLoginDataModel model)
        {
            try
            {
                await _unitOfWork.UserLoginRepository.Update(model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
