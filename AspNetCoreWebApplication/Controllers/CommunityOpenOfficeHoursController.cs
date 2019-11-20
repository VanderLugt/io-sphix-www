using System;
using System.IO;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using CustomTwilioClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.OpenOfficeHours;
using Sphix.Service.UserCommunities.OpenOfficeHours.OpenOfficeHoursThanksMail;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class CommunityOpenOfficeHoursController : Controller
    {
        private readonly IOpenOfficeHoursService _openOfficeHoursService;
        private readonly IOpenHoursMailService _openHoursMailService;
        private readonly IHostingEnvironment _env;
       
        private ClaimAccessor _claimAccessor;

        public CommunityOpenOfficeHoursController(IOpenOfficeHoursService openOfficeHoursService
            , ClaimAccessor claimAccessor
            , IOpenHoursMailService openHoursMailService
            , IHostingEnvironment env
            )
        {
            _openOfficeHoursService = openOfficeHoursService;
            _claimAccessor = claimAccessor;
            _openHoursMailService = openHoursMailService;
            _env = env;
            
        }
        public async Task<IActionResult> ActiveNextSession(string Id)
        {
            OpenOfficeHoursViewModel model = new OpenOfficeHoursViewModel();
            if (!string.IsNullOrEmpty(Id))
            {
                var openHoursModel = await _openOfficeHoursService.getOpenHoursAsync(Id);
                if(openHoursModel!=null && openHoursModel.IsMeetingTokenUsed==false)
                {
                    //DateTime nextMeetingDate = openHoursModel.OFromDate.AddDays(7);
                   
                    model.UserId = openHoursModel.CreatedBy;
                    model.MaxAttendees =  openHoursModel.MaxAttendees;
                    model.ODescription = openHoursModel.ODescription;
                    model.OFrequency = openHoursModel.OFrequency;
                    TimeSpan difference = DateTime.Now.Date - openHoursModel.OFromDate.Date;
                    if (difference.Days > 7)
                    {
                        model.OFromDate = SphixHelper.setDateFromDayName(openHoursModel.OTimeDayName, DateTime.Now.Date);
                    }
                    else if (difference.Days == 0)
                    {
                        model.OFromDate = openHoursModel.OFromDate;
                    }
                    else
                    {
                        model.OFromDate = SphixHelper.setDateFromDayName(openHoursModel.OTimeDayName, openHoursModel.OFromDate);
                    }
                   // model.OFromDate = nextMeetingDate;
                    model.OTimeZone = openHoursModel.OTimeZone;
                    model.OTime = openHoursModel.OTime;
                    model.OToDate = model.OFromDate;// openHoursModel.OToDate;
                    model.OName = openHoursModel.OName;
                    model.OTime = openHoursModel.OTime;
                    model.OTitle = openHoursModel.OTitle;
                    model.WhoCanAttend = openHoursModel.WhoCanAttend;
                    model.OTimeDayName = openHoursModel.OTimeDayName;
                    model.CommunityGroupId = openHoursModel.CommunityGroups.Id;
                    string token = Guid.NewGuid().ToString().Replace("-", "");
                    model.NextMeetingToken = token;
                    model.IsFirstMeeting = false;
                    model.LastSessionId = openHoursModel.Id;
                    var _existingTable = await _openOfficeHoursService.CheckTableIsExist(model.UserId, model.CommunityGroupId, model.OTimeZone, model.OFromDate);
                    if (_existingTable.Status)
                    {
                        return Json(_existingTable);
                    }
                    var _result = await _openOfficeHoursService.SaveOpenHoursAsync(model, null, null);
                    if (_result.Status)
                    {
                       await _openOfficeHoursService.UpdateNextMeetingToken(openHoursModel.Id);
                        //IsMeetingTokenUsed
                        var pathToFile = _env.WebRootPath
                                 + Path.DirectorySeparatorChar.ToString()
                                 + "Templates"
                                 + Path.DirectorySeparatorChar.ToString()
                                 + "EmailTemplates"
                                 + Path.DirectorySeparatorChar.ToString()
                                 + "OpenHoursMeetingCreateNewTable.html";
                        string HtmlBody = string.Empty;
                        using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                        {
                            HtmlBody = SourceReader.ReadToEnd();
                        }
                        var callbackUrl = Url.Action("ActiveNextSession", "CommunityOpenOfficeHours", null, protocol: HttpContext.Request.Scheme) + "/" + token;
                      await  _openHoursMailService.SendMailOnCreateNewTableAsync(model.UserId, token, callbackUrl, HtmlBody, model.OFromDate.ToShortDateString() +" at "+model.OTime+" "+model.OTimeZone);
                        //send thanks mail
                    }

                }
                else
                {
                    model.IsMeetingTokenUsed = true;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> AddOpenOfficeHours(OpenOfficeHoursViewModel model, long OpenHoursId)
        {
            if (!ModelState.IsValid)
            {
                return Json(new BaseModel { Status=false,Messsage=UMessagesInfo.Error});
            }
            var openHoursModel = await _openOfficeHoursService.getOpenHoursAsync(OpenHoursId);

            model.UserId = _claimAccessor.UserId;
            model.MaxAttendees = openHoursModel.MaxAttendees;
            model.ODescription = openHoursModel.ODescription;
            model.OFrequency = openHoursModel.OFrequency;
            TimeSpan difference = DateTime.Now.Date - openHoursModel.OFromDate.Date;
            if (difference.Days>7)
            {
                model.OFromDate = SphixHelper.setDateFromDayName(openHoursModel.OTimeDayName, DateTime.Now.Date);
            }
           else if(difference.Days==0)
            {
                model.OFromDate = openHoursModel.OFromDate;
            }
            else
            {
                model.OFromDate = SphixHelper.setDateFromDayName(openHoursModel.OTimeDayName, openHoursModel.OFromDate);
            }

            model.OToDate = model.OFromDate;
          //  model.OName = openHoursModel.OName;
            model.OTime = openHoursModel.OTime;
            //model.OTitle = openHoursModel.OTitle;
            model.WhoCanAttend = openHoursModel.WhoCanAttend;
            model.OTimeDayName = openHoursModel.OTimeDayName;
            model.IsFirstMeeting = false;
            string token = Guid.NewGuid().ToString().Replace("-", "");
            model.NextMeetingToken = token;
            model.LastSessionId = 0;
            var _existingTable = await _openOfficeHoursService.CheckTableIsExist(_claimAccessor.UserId, model.CommunityGroupId, model.OTimeZone, model.OFromDate);
            if (_existingTable.Status)
            {
                return Json(  new BaseModel { Status = false, Messsage = UMessagesInfo.RecordExist });
            }
            var _result = await _openOfficeHoursService.SaveOpenHoursAsync(model, null, null);
            if (_result.Status)
            {
                var pathToFile = _env.WebRootPath
                         + Path.DirectorySeparatorChar.ToString()
                         + "Templates"
                         + Path.DirectorySeparatorChar.ToString()
                         + "EmailTemplates"
                         + Path.DirectorySeparatorChar.ToString()
                         + "OpenHoursMeetingCreateNewTable.html";
                string HtmlBody = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = SourceReader.ReadToEnd();
                }
                var callbackUrl = Url.Action("ActiveNextSession", "CommunityOpenOfficeHours", null, protocol: HttpContext.Request.Scheme) + "/"+ token;
               await _openHoursMailService.SendMailOnCreateNewTableAsync(_claimAccessor.UserId, token, callbackUrl, HtmlBody,model.OFromDate.ToShortDateString()+" at "+model.OTime+" "+model.OTimeZone);
                //send thanks mail
            }
            return Json(_result);
        }
     

    }
}
