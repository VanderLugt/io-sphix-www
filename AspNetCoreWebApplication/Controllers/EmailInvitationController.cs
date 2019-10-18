using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.EmailInvitation;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.EmailInvitation;
using System.IO;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class EmailInvitationController : Controller
    {
        private readonly IEmailInvitationService _emailInvitationService;
        private ClaimAccessor _claimAccessor;
        private readonly IHostingEnvironment _env;
        public EmailInvitationController(IEmailInvitationService emailInvitationService
            , ClaimAccessor claimAccessor
            , IHostingEnvironment env
            )
        {
            _emailInvitationService = emailInvitationService;
            _claimAccessor = claimAccessor;
            _env = env;
        }
        public async Task<IActionResult> SendGroupEmailInvitation(GroupEmailInvitationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json( new BaseModel { Status = false, Messsage = UMessagesInfo.Error });
            }
            model.SentByUser = _claimAccessor.UserId;
            var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "GroupEmailInvitation.html";
            string HtmlBody = string.Empty;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                HtmlBody = SourceReader.ReadToEnd();
            }
            var callbackUrl = Url.Action("JoinGroupFromInvitation", "EmailInvitation", null, protocol: HttpContext.Request.Scheme) + "/" ;
            return Json(await _emailInvitationService.sendGroupEmailInvitation(model, HtmlBody, callbackUrl));
        }
        public async Task<IActionResult> JoinGroupFromInvitation(string Id)
        {
            var result = await _emailInvitationService.checkGroupInvitationToken(Id, _claimAccessor.UserId);
            if(result.Status)
            {
                return RedirectToAction(result.Messsage, "CommunityGroup" );
            }
            return RedirectToAction("Login","Home");
        }
    }
}