using System.IO;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.User.UserCommunities;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using Sphix.Service.UserCommunities.CommunityGroupPublishMail;
using Sphix.Service.UserCommunities.JoinCommunityEventMeeting;
using Sphix.Service.UserCommunities.JoinCommunityGroup;
using Sphix.Service.UserCommunities.JoinCommunityOpenHoursMeeting;
using Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursThanksMail;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class CommunityGroupSubSectionsController : Controller
    {
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        private ClaimAccessor _claimAccessor;
        private readonly IJoinCommunityGroupService _joinCommunityGroupService;
        private readonly IJoinCommunityOpenHoursMeetingService _joinCommunityOpenHoursMeetingService;
        private readonly IJoinCommunityEventMeetingService _joinCommunityEventMeetingService;
        private readonly IUserCommunitiesService _userCommunitiesService;
        private readonly IHostingEnvironment _env;
        private readonly ICommunityGroupEmailService _communityGroupEmailService;
        private readonly IOpenHoursMailService _openHoursMailService;
        public CommunityGroupSubSectionsController(ClaimAccessor claimAccessor
            ,ICommunitiesForGoodService communitiesForGoodService
            , IJoinCommunityGroupService joinCommunityGroupService
            , IJoinCommunityOpenHoursMeetingService joinCommunityOpenHoursMeetingService
            , IJoinCommunityEventMeetingService joinCommunityEventMeetingService
            , IUserCommunitiesService userCommunitiesService
            , IHostingEnvironment env
            , ICommunityGroupEmailService communityGroupEmailService
              , IOpenHoursMailService openHoursMailService
         )
        {
            _env = env;
            _communityGroupEmailService = communityGroupEmailService;
            _communitiesForGoodService = communitiesForGoodService;
            _claimAccessor = claimAccessor;
            _joinCommunityGroupService = joinCommunityGroupService;
            _joinCommunityOpenHoursMeetingService = joinCommunityOpenHoursMeetingService;
            _joinCommunityEventMeetingService = joinCommunityEventMeetingService;
            _userCommunitiesService = userCommunitiesService;
            _openHoursMailService = openHoursMailService;
        }
        
        public async Task<IActionResult> CommunityGroupDetail(long Id)
        {
            return PartialView("_communityGroupDetail", await _communitiesForGoodService.getCommunityGroupDetail(Id));
        }
        #region join sections like community group, open hours meeting
        [HttpPost]
        public async Task<IActionResult> JoinCommunity(JoinCommunityGroupViewModel model)
        {
            var _result = await _userCommunitiesService.JoinCommunitiesAsync(model.CommunityGroupId, _claimAccessor.UserId);
            if (_result.Status)
            {
                var pathToFile = _env.WebRootPath
                       + Path.DirectorySeparatorChar.ToString()
                       + "Templates"
                       + Path.DirectorySeparatorChar.ToString()
                       + "EmailTemplates"
                       + Path.DirectorySeparatorChar.ToString()
                       + "JoinCommunityWelcome.html";
                string HtmlBody = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = SourceReader.ReadToEnd();
                }
                await _communityGroupEmailService.SendJoinCommunityEmailAsync(_claimAccessor.UserId, model.CommunityGroupId, HtmlBody);
            }
            return Json(_result);
        }
        [HttpPost]
        public async Task<IActionResult> JoinCommunityGroup(JoinCommunityGroupViewModel model)
        {
            model.UserId = _claimAccessor.UserId;
            return Json(await _joinCommunityGroupService.JoinCommunityGroupAsync(model));
        }
        [HttpPost]
        public async Task<IActionResult> JoinCommunityOpenHoursMeeting(JoinCommunityOpenHoursMeetingViewModel model)
        {
            if (UMessagesInfo.CheckTimeZoneValidation(model.TimeZone))
            {
                model.UserId = _claimAccessor.UserId;
                var _result = await _joinCommunityOpenHoursMeetingService.SaveAsync(model);
                if (_result.Status)
                {
                    var pathToFile = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Templates"
                             + Path.DirectorySeparatorChar.ToString()
                             + "EmailTemplates"
                             + Path.DirectorySeparatorChar.ToString()
                             + "WelcomJoinOpenHoursMeeting.html";
                    string HtmlBody = string.Empty;
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        HtmlBody = SourceReader.ReadToEnd();
                    }
                    await _openHoursMailService.WelcomeMailOnJoinOpenOfficeHoursMettingAsync(_claimAccessor.UserId,  HtmlBody, _result.Data);
                    //send thanks mail
                }
                return Json(_result);
            }
            else
            {
                return Json(new BaseModel {Status=false,Messsage=UMessagesInfo.Error });
            }
        }
        [HttpPost]
        public async Task<IActionResult> JoinCommunityEventMeeting(JoinCommunityOpenHoursMeetingViewModel model)
        {
            if (UMessagesInfo.CheckTimeZoneValidation(model.TimeZone))
            {
                model.UserId = _claimAccessor.UserId;
                var _result = await _joinCommunityEventMeetingService.SaveAsync(model);
                if (_result.Status)
                {
                    var pathToFile = _env.WebRootPath
                             + Path.DirectorySeparatorChar.ToString()
                             + "Templates"
                             + Path.DirectorySeparatorChar.ToString()
                             + "EmailTemplates"
                             + Path.DirectorySeparatorChar.ToString()
                             + "WelcomJoinEventMeeting.html";
                    string HtmlBody = string.Empty;
                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        HtmlBody = SourceReader.ReadToEnd();
                    }
                    await _openHoursMailService.WelcomeMailOnJoinEventMettingAsync(_claimAccessor.UserId, HtmlBody, _result.Data);
                    //send thanks mail
                }
                return Json(_result);
            }
            else
            {
                return Json(new BaseModel { Status = false, Messsage = UMessagesInfo.Error });
            }
        }
        #endregion
        public async Task<IActionResult> CommunityGroupEvents(string Id)
        {
            EventListSearchFilter filter = new EventListSearchFilter {
                CommunityGroupsId=Id,
                PageLimit="100",
                PageNumber="0",
                OrderBy= "EFromDate desc",
                SearchValue=""
            };
            return PartialView("_communityGroupEvents", await _communitiesForGoodService.getActiveCommunityGroupEventsListAsync(filter));
        }
        public async Task<IActionResult> CommunityGroupArticels(string Id)
        {
            ViewBag.CommunityGroupId = Id;
            EventListSearchFilter filter = new EventListSearchFilter
            {
                CommunityGroupsId = Id,
                PageLimit = "100",
                PageNumber = "0",
                OrderBy = "AddedDate desc",
                SearchValue = ""
            };
            return PartialView("_communityGroupArticles", await _communitiesForGoodService.getActiveCommunityGroupArticlesListAsync(filter));
        }
        public async Task<IActionResult> RegisterForOpenHours(long Id,string TimeZone)
        {
            ViewBag.TimeZone = TimeZone;
            return PartialView("_registerForOpenHours", await _communitiesForGoodService.getCommunityGroupOpenHoursDetail(Id));
        }
        public async Task<IActionResult> OpenOfficeHoursTables(long Id, string TimeZone)
        {
            ViewBag.TimeZone = TimeZone;
            return PartialView("_registerForOpenHoursTables", await _communitiesForGoodService.getOpenOfficeHoursTables(Id,TimeZone));
        }
        public async Task<IActionResult> RegisterForEvent(long Id, string TimeZone)
        {
            ViewBag.TimeZone = TimeZone;
            return PartialView("_registerForEvent", await _communitiesForGoodService.getCommunityGroupEventDetail(Id));
        }
        public async Task<IActionResult> ArticleDetail(long Id)
        {
            return PartialView("_articleDetail", await _communitiesForGoodService.getArticleDetailAsync(Id));
        }
        public IActionResult SingleEvent()
        {
            return PartialView("_liveSingleEventEdit");
        }
    }
}