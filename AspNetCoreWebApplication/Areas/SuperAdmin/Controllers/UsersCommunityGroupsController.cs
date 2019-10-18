using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities;
using Sphix.ViewModels;

namespace AspNetCoreWebApplication.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [CustomAuthorizeAttribute]
    public class UsersCommunityGroupsController : Controller
    {
        private readonly ICommunityGroupsService _communityGroupsService;
        public UsersCommunityGroupsController(ICommunityGroupsService communityGroupsService) {
            _communityGroupsService = communityGroupsService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CommunitiesGroupsList(CustomeSearchFilter model)
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

            model.PageLimit = length;
            model.PageNumber = start;
            model.SearchValue = searchValue;
            model.OrderBy = sortColumn + " " + sortColumnDirection;
            var data = await _communityGroupsService.getAdminCommunitiesGroupsList(model);
            int recordsTotal = 0;
            if (data.Count != 0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        public async Task<IActionResult> ViewCommunityGroup(long Id)
        {
            //ViewBag.Communities = await _communitiesService.GetActiveCommunities();
            ViewBag.Id = Id;
            return PartialView("_viewCommunityGroup", await this._communityGroupsService.getCommunityGroupDetailAdmin(Id));
        }
        [HttpPost]
        public async Task<JsonResult> PublishCommunityGroup(long Id, bool IsPublish)
        {
            return Json(await _communityGroupsService.PublishCommunityGroupAsync(Id, IsPublish));
        }
    }
}