using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Communities;
using Sphix.Service.UserCommunities;
using Sphix.Service.UserCommunities.CommunityGroupPublishMail;
using Sphix.ViewModels;
using Sphix.ViewModels.UserCommunities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class ManageCommunityGroupsController : Controller
    {
        private ClaimAccessor _claimAccessor;
        private readonly ICommunitiesService _communitiesService;
        //ICommunityGroupsService for create new community groups
        private readonly ICommunityGroupsService _communityGroupsService;
        private readonly ICommunityGroupEventService _communityGroupEventService;
        private readonly ICommunityGroupArticleService _communityGroupArticleService;
        private readonly IHostingEnvironment _env;
        private readonly ICommunityGroupEmailService _communityGroupEmailService;
        public ManageCommunityGroupsController(ClaimAccessor claimAccessor,ICommunitiesService communitiesService,
            ICommunityGroupsService communityGroupsService
            , ICommunityGroupEventService communityGroupEventService
            , ICommunityGroupArticleService communityGroupArticleService
            ,IHostingEnvironment env
            , ICommunityGroupEmailService communityGroupEmailService
            )
        {
            _communitiesService = communitiesService;
            _communityGroupsService = communityGroupsService;
            _claimAccessor = claimAccessor;
            _env = env;
            _communityGroupEventService = communityGroupEventService;
            _communityGroupArticleService = communityGroupArticleService;
            _communityGroupEmailService = communityGroupEmailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Edit(long Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public async Task<IActionResult> AddCommunityGroupSteps()
        {
            ViewBag.Communities = await _communitiesService.GetActiveCommunities();
           
            return PartialView("_addCommunityGroupSteps");
        }
        public async Task<JsonResult> getCommunityDetail(long Id)
        {
            return Json(await _communityGroupsService.getCommunityGroupDetail(Id, _claimAccessor.UserId));
        }
        public async Task<IActionResult> EditCommunityGroup(long Id)
        {
            ViewBag.Communities = await _communitiesService.GetActiveCommunities();
            ViewBag.Id = Id;
            return PartialView("_editCommunityGroupSteps");
        }
        public async Task<IActionResult> TargetedCommunityGroups(int CommunityId)
        {
            return PartialView("Views/ManageCommunityGroups/_targetedCommunityGroups.cshtml", await _communitiesService.GetActiveSubCommunities(CommunityId));
        }
        public async Task<IActionResult> TargetedCommunityThemes(int CommunityId)
        {
            return PartialView("Views/ManageCommunityGroups/_targetedCommunityThemes.cshtml", await _communitiesService.GetActiveCommunityThemes(CommunityId));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(500000000)]
        public async Task<JsonResult> AddCommunityGroupSteps(CommunityGroupViewModel model, IFormFile file, IFormFile articleShareDocument)
        {
          
            model.UserId = this._claimAccessor.UserId;
            
            BaseModel result = await _communityGroupsService.SaveAsync(model, file, articleShareDocument);
            if (result.Status && model.Id==0)
            {
                var pathToFile = _env.WebRootPath
                        + Path.DirectorySeparatorChar.ToString()
                        + "Templates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "EmailTemplates"
                        + Path.DirectorySeparatorChar.ToString()
                        + "NewCommunityGroup.html";
                string HtmlBody = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    HtmlBody = SourceReader.ReadToEnd();
                }
                var callbackUrl = Url.Action("CommunityGroupVerification", "Shx", null, protocol: HttpContext.Request.Scheme) + "/";
                await _communityGroupEmailService.SendCommunityGroupPublishEmailAsync(result.Id, HtmlBody, callbackUrl);
            }
            return Json(result);
            //return null;
        }
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(500000000)]
        public async Task<JsonResult> EditCommunityGroupSteps(CommunityGroupViewModel model, IFormFile file, IFormFile articleShareDocument)
        {
            model.UserId = this._claimAccessor.UserId;
            return Json(await _communityGroupsService.SaveAsync(model, file, articleShareDocument));
            //return null;
        }
      
        public IActionResult MyCommunitiesGroups()
        {
            return PartialView("_communitiesList");
        }
        public async Task<IActionResult> MyCommunitiesGroupsList(SearchFilter model)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            model.Id = _claimAccessor.UserId;
            model.PageLimit = length;
            model.PageNumber = start;
            model.SearchValue = searchValue;
            model.OrderBy= sortColumn+" "+ sortColumnDirection;
            var data = await _communityGroupsService.getCommunitiesGroupsList(model);
            int recordsTotal = 0;
            if(data.Count!=0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        #region events
        public IActionResult MyCommunityGroupEvents(long Id)
        {
            ViewBag.Id = Id;
            return PartialView("_eventsList");
        }
        public async Task<IActionResult> CommunityGroupEventEdit(long Id,long CommunityGroupId)
        {
            ViewBag.Id = Id;
            ViewBag.CommunityGroupId = CommunityGroupId;
            return PartialView("_liveEventsEdit",await _communityGroupEventService.getEventDetailAsync(Id));
        }
        [HttpPost]
        public async Task<JsonResult> SaveCommunityGroupEvent(LiveEventViewModel model)
        {
            model.UserId = this._claimAccessor.UserId;
            return Json(await _communityGroupEventService.SaveEventAsync(model));
        }
        public async Task<IActionResult> MyCommunityGroupEventsList(EventListSearchFilter model)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            model.Id = _claimAccessor.UserId;
            model.PageLimit = length;
            model.PageNumber = start;
            model.SearchValue = searchValue;
            model.OrderBy = sortColumn + " " + sortColumnDirection;
            var data = await _communityGroupEventService.getUserCommunityGroupEventsListAsync(model);
            int recordsTotal = 0;
            if (data.Count != 0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        #endregion events
        #region article
        public IActionResult MyCommunityGroupArticles(long Id)
        {
            ViewBag.Id = Id;
            return PartialView("_articlesList");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(500000000)]
        public async Task<JsonResult> SaveCommunityGroupArticle(ArticleViewModel model, IFormFile articleShareDocument)
        {
            model.UserId = this._claimAccessor.UserId;
            return Json(await _communityGroupArticleService.SaveAsync(model,  articleShareDocument));
            //return null;
        } 
        public async Task<IActionResult> CommunityGroupArticleEdit(long Id, long CommunityGroupId)
        {
            ViewBag.Id = Id;
            ViewBag.CommunityGroupId = CommunityGroupId;
            return PartialView("_articleEdit", await _communityGroupArticleService.getArticleDetailAsync(Id));
        }
        public async Task<IActionResult> MyCommunityGroupArticlesList(EventListSearchFilter model)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            model.Id = _claimAccessor.UserId;
            model.PageLimit = length;
            model.PageNumber = start;
            model.SearchValue = searchValue;
            model.OrderBy = sortColumn + " " + sortColumnDirection;
            var data = await _communityGroupArticleService.getUserCommunityGroupArticlesListAsync(model);
            int recordsTotal = 0;
            if (data.Count != 0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        #endregion
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[DisableFormValueModelBinding]
        //[RequestSizeLimit(500000000)]
        //public async Task<IActionResult> UploadStreamingFile()
        //{
        //    return Ok();
        //}
    }
   
}