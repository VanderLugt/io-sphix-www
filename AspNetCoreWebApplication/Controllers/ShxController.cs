using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Authorization.Login.ForgotPassword;
using Sphix.Service.Authorization.SignUp.EmailVerification;
using Sphix.Service.User;
using Sphix.Service.UserCommunities.CommunityGroupPublishMail;
using Sphix.ViewModels.User;

namespace Sphix.Web.Controllers
{
    public class ShxController : Controller
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly IUserService _userService;
        private readonly IHostingEnvironment _env;
        private readonly ICommunityGroupEmailService _communityGroupEmailService;
        public ShxController(IEmailVerificationService emailVerificationService
            ,IHostingEnvironment hostingEnvironment
            , IForgotPasswordService forgotPasswordService, IUserService userService
            , ICommunityGroupEmailService communityGroupEmailService)
        {
            _emailVerificationService = emailVerificationService;
            _env = hostingEnvironment;
            _forgotPasswordService = forgotPasswordService;
            _userService = userService;
            _communityGroupEmailService = communityGroupEmailService;
        }
        public async Task<IActionResult> EmailVerification(string id)
        {
            UserShortProfileViewModel model = await _emailVerificationService.ValidateToken(id);
            if (model.Status) { 
            var webRoot = _env.WebRootPath; //get wwwroot Folder  
                                            //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Confirm_Your_Email.html  
            var pathToFile = _env.WebRootPath
                    + Path.DirectorySeparatorChar.ToString()
                    + "SphixFirsttimeuse.html";
            string HtmlBody = string.Empty;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                HtmlBody = SourceReader.ReadToEnd();
            }
                HtmlBody = HtmlBody.Replace("@Name", model.FirstName+' '+model.LastName);
            ViewBag.SphixFirsttimeuse = HtmlBody;
            return View(model);
            }
            return View(model);
        }
        public async Task<IActionResult> RestPassword(string id)
        {
            ViewBag.Token = id;
            return View(await _forgotPasswordService.ValidateTokenAsync(id));
        }
        public async Task<IActionResult> CommunityGroupVerification(string id)
        {
            //
            return View(await _communityGroupEmailService.CommunityGroupVerificationAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ResetPaswordViewModel model)
        {
            return Json(await _userService.ChangePasswordAsync(model));
        }
    }
}