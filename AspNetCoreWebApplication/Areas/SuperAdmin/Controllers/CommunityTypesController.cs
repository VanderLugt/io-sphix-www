using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Communities;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;

namespace AspNetCoreWebApplication.Areas.Admin.Controllers
{
    [Area("SuperAdmin")]
    [CustomAuthorizeAttribute]
    public class CommunityTypesController : Controller
    {
        public readonly ICommunitiesService _communitiesService;
        public CommunityTypesController(ICommunitiesService communitiesService)
        {
            _communitiesService = communitiesService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Add(int Id)
        {
            //ViewBag.Communities = await _communitiesService.GetActiveCommunities();
            ViewBag.Id = Id;
            return PartialView("_editCommunityType", await this._communitiesService.getCommunityTypeAsync(Id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(500000000)]
        public async Task<JsonResult> SaveCommunityType(CommunityTypeViewModel model, IFormFile File)
        {
            return Json(await _communitiesService.SaveAsync(model, File));
            //return null;
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
            var data = await _communitiesService.getCommunitiesGroupsTypeList(model);
            int recordsTotal = 0;
            if (data.Count != 0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}