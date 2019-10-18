using System.IO;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Authorization.SignUp.EmailVerification;
using Sphix.Service.Communities;
using Sphix.Service.User;
using Sphix.Service.User.Associations;
using Sphix.Service.User.Notification;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class DashboardController : Controller
    {
        private ClaimAccessor _claimAccessor;
        private readonly IUserService _userService;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IHostingEnvironment _env;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly ICommunitiesService _communitiesService;
        private readonly IAssociationsSettingService _associationsSettingService;
        public DashboardController(ClaimAccessor claimAccessor
            ,IUserService userService
            ,INotificationSettingsService notificationSettingsService
            ,IHostingEnvironment env
            , IEmailVerificationService emailVerificationService
            , ICommunitiesService communitiesService
            , IAssociationsSettingService associationsSettingService
            )
        {
            _userService = userService;
            _claimAccessor = claimAccessor;
            _notificationSettingsService = notificationSettingsService;
             _emailVerificationService = emailVerificationService;
            _env = env;
            _communitiesService = communitiesService;
            _associationsSettingService = associationsSettingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Notification()
        {
            return View();
        }
        #region Update Profile
        public async Task<IActionResult> UpdateProfile()
        {
            ViewBag.HostUrl = Url.Action("", "profile", null, protocol: HttpContext.Request.Scheme);
            return PartialView("_UpdateProfileInfo", await _userService.GetUserProfileByIdAsync(this._claimAccessor.UserId));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserProfileViewModel model)
        {
            model.LogedInUserId = _claimAccessor.UserId;
            return Json(await _userService.UpdateProfileAsync(model));
        }
        [HttpPost]
        public async Task<JsonResult> UpdateProfilePicture(IFormFile file)
        {
             return Json(await _userService.UpdateProfilePictureAsync(file,this._claimAccessor.UserId));
        }
        #endregion Update Profile
        #region User settings 
        public async Task<IActionResult> Settings()
        {
            return PartialView("_Settings",await _notificationSettingsService.GetNotificationSettingsAsync(_claimAccessor.UserId));
        }
        //save notification settings
        [HttpPost]
        public async Task<IActionResult> Settings(NotificationSettingsViewModel model)
        {
            model.UserId = _claimAccessor.UserId;
            return Json(await _notificationSettingsService.SaveAsync(model));
        }
        public async Task<IActionResult> CommunityAssociations()
        {
            return PartialView("_communityAssociations",await _associationsSettingService.getUserAssociationsAsync(_claimAccessor.UserId));
        }
        public async Task<IActionResult> EditAssociations(int CommunityId)
        {
            return PartialView("_editAssociations", await _associationsSettingService.getEditUserAssociationsAsync(CommunityId,_claimAccessor.UserId));
        }
        public async Task<IActionResult> SaveAssociations(AssociationsModel model)
        {
            model.UserId = _claimAccessor.UserId;
            return Json(await _associationsSettingService.SaveAssociationsAsync(model));
        }
        #endregion User settings 
        [HttpPost]
        public async Task<IActionResult> ReSendVerification()
        {
            var result = await _userService.GetUserShortProfileById(_claimAccessor.UserId);
            if (result.Status)
            {
                var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Confirm_Your_Email.html";
                string HtmlBody = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = SourceReader.ReadToEnd();
                }
                var callbackUrl = Url.Action("EmailVerification", "Shx", null, protocol: HttpContext.Request.Scheme) + "/";
                return Json(await _emailVerificationService.SendEmailVerification(_claimAccessor.UserId, HtmlBody, callbackUrl, result.FirstName + ' ' + result.LastName));
            }
            return Json(new BaseModel { Status = false, Messsage = UMessagesInfo.Error });
        }
    }
}