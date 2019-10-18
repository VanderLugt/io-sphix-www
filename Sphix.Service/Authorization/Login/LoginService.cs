using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sphix.DataModels.User;
using Sphix.Service.Logger;
using Sphix.UnitOfWorks;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sphix.Service.Authorization.Login
{
    public class LoginService : ILoginService
    {
        private UnitOfWork _unitOfWork;
        private readonly PasswordSettings _settings;
        private readonly ILoggerService _loggerService;
        public LoginService(EFDbContext context, IOptions<PasswordSettings> settings, ILoggerService loggerService)
        {
            _unitOfWork = new UnitOfWork(context);
            _settings = settings.Value;
            _loggerService = loggerService;
        }
        public async Task<UserClaimsViewModel> LoginAsync(LoginViewModel model)
        {
            IList<UsersLoginDataModel> users= await _unitOfWork.UserLoginRepository.FindAllBy(c => c.UserName.ToLower() == model.UserName);
            UsersLoginDataModel userData = users.FirstOrDefault();
            if (userData != null)
            {

                try
                {
                    string userPassword = UPasswordRijndaelAlgorithm.Decrypt(userData.Password, userData.PasswordSalt, _settings.PassPhrase, _settings.HashAlgorithm, _settings.InitVector);
                    if (model.Password.Equals(userPassword))
                    {
                        var _query = _unitOfWork.UsersRolesRepository.FindAllByQuery(c => c.User == userData);
                        var _result = await _query.Include("Role").ToListAsync();
                        if (_result != null)
                        {
                            string roles = string.Empty;
                            foreach (var item in _result)
                            {
                                roles = roles + item.Role.RoleName;// + ",";
                            }
                            return new UserClaimsViewModel { Status = true, Id = userData.Id, UserName = userData.UserName,Roles= roles };
                        }
                        else
                        {
                            return new UserClaimsViewModel { Status = false, Messsage = UMessagesInfo.WrongUserName };
                        }
                    }
                    return new UserClaimsViewModel { Status = false, Messsage = UMessagesInfo.WrongUserName };
                }
                catch (System.Exception ex)
                {
                   
                    if (ex.Source== "System.Security.Cryptography.Algorithms")
                    {
                        //await _loggerService.AddAsync(new Sphix.DataModels.Logger.LoggerDataModel
                        //{
                        //    ErrorCode = "red",
                        //    Message = ex.Message,
                        //    Source = ex.Source,
                        //    Detail = ex.StackTrace
                        //});
                        return new UserClaimsViewModel { Status = false, Messsage = "System security settings have changed. Please reset your password!" };
                    }
                    throw;
                }
            }
            else
            {
                return new UserClaimsViewModel { Status=false,Messsage= UMessagesInfo.UserNameNotExist };
            }
        }
    }
}
