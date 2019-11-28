using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sphix.Service.Authorization;
using Sphix.Service.Authorization.Login;
using Sphix.Service.Authorization.Login.ForgotPassword;
using Sphix.Service.Authorization.SignUp.EmailVerification;
using Sphix.Service.Communities;
using Sphix.Service.CronJob;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.User;

namespace Sphix.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICommunitiesService _communitiesService;
        private readonly ISignUpService _SignUpService;
        private readonly IHostingEnvironment _env;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly ILoginService _loginService;
        private readonly IForgotPasswordService _forgotPasswordService;
        public HomeController(ICommunitiesService communitiesService,ISignUpService SignUpService,
            IHostingEnvironment env, IEmailVerificationService emailVerificationService,
            ILoginService loginService,
            IForgotPasswordService forgotPasswordService
           )
        {
            _communitiesService = communitiesService;
            _SignUpService = SignUpService;
            _emailVerificationService = emailVerificationService;
            _env = env;
            _loginService = loginService;
            _forgotPasswordService = forgotPasswordService;
        }
        public async Task<IActionResult> Index(string returnUrl="")
        {
            //  var room1 = RoomResource.Fetch(pathSid: "RM87993f8e419e87639462efaa9f1bf2ae");

            //Console.WriteLine(room.Sid);
            //var verification = await VerificationResource.CreateAsync(
            //     pathServiceSid: _settings.VerificationServiceSID,
            //     to: "+91XXXXXXX7",
            //     channel: "sms");
            //if(verification.Status=="Pending")
            //{ }
         
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "CommunitiesForGood");
            }
            ViewBag.Communities =  await _communitiesService.GetActiveCommunities();
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _loginService.LoginAsync(model);
            if(result.Status)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.UserName),
                        new Claim(ClaimTypes.Role, result.Roles),
                        new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                };

                ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                User.HasClaim(ClaimTypes.Role, result.Roles);

                await HttpContext.SignInAsync(principal);
            }
             return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> SendForgotPasswordLink(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var webRoot = _env.WebRootPath; //get wwwroot Folder  
                                                //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Confirm_Your_Email.html  
                var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Reset_Password.html";
                string HtmlBody = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = SourceReader.ReadToEnd();
                }
                var callbackUrl = Url.Action("RestPassword", "Shx", null, protocol: HttpContext.Request.Scheme)+"/";

               return Json(await _forgotPasswordService.SendForgotPasswordLinkAsync(model.UserName,HtmlBody,callbackUrl));
            }
            return Json(new BaseModel { Status = false, Messsage = UMessagesInfo.Error });
        }
        public async Task<IActionResult> SignUpStep3(int CommunityId)
        {
            return PartialView("_SignUpStep3", await _communitiesService.GetActiveSubCommunities(CommunityId));
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                BaseModel result = await _SignUpService.SignUpAsync(model);
                if(result.Status)
                {
                    //var webRoot = _env.WebRootPath; //get wwwroot Folder  
                    //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Confirm_Your_Email.html  
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
                    var callbackUrl = Url.Action("EmailVerification", "Shx", null, protocol: HttpContext.Request.Scheme)+"/";
                    await _emailVerificationService.SendEmailVerification(result.Id, HtmlBody, callbackUrl,model.FirstName +' '+ model.LastName);
                }
                return Json(result);
            }
            return Json(new BaseModel {Status=false,Messsage=UMessagesInfo.Error});
        }
        public IActionResult Error()
        {
            string RequestId = string.Empty;
            int statusCode = HttpContext.Response.StatusCode;
           
            RequestId= Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            string uaString = HttpContext.Request.Headers["User-Agent"].ToString();

            var uid = "Unknown";
            if (User.Identity.IsAuthenticated)
            {
                uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            StringBuilder sb = new StringBuilder($"An error has occurred on {HttpContext.Request.Host}. \r\n \r\n");
            sb.Append($"RequestId = {RequestId} \r\nStatusCode = {statusCode.ToString()} \r\n");

            sb.Append($"OriginalPath = {feature?.OriginalPath} \r\n \r\n");

            sb.Append($"Path = {Request.Path}. \r\n \r\n");

            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exception != null)
            {
                sb.Append($"Error Message = {exception.Error.Message}. \r\n");
                sb.Append($"Error Source = {exception.Error.Source}. \r\n");

                if (exception.Error.InnerException != null)
                    sb.Append($"Inner Exception = {exception.Error.InnerException.ToString()}. \r\n");
                else
                    sb.Append("Inner Exception = null. \r\n");

                sb.Append($"Error StackTrace = {exception.Error.StackTrace}. \r\n");
            }
            ViewData["Message"] = sb.ToString();
            //  await _emailVerificationService.SendEmailVerification($"Error on {HttpContext.Request.Host}.", sb.ToString(), uaString, ipAnonymizedString, uid);
            return View();
        }

    }
}
