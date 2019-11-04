using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sphix.Service.Communities;
using Sphix.Service.Communities.ComunitySubTypes;
using Sphix.Utility;
using Sphix.ViewModels;
using Sphix.ViewModels.Communities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [CustomAuthorizeAttribute]
    public class CommunitySubTypesController : Controller
    {
        private readonly IComunitySubTypesService _comunitySubTypesService;
        public readonly ICommunitiesService _communitiesService;
        public CommunitySubTypesController(IComunitySubTypesService comunitySubTypesService, ICommunitiesService communitiesService)
        {
            _comunitySubTypesService = comunitySubTypesService;
            _communitiesService = communitiesService;
        }
        public async Task<IActionResult>  Index(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "CommunityTypes",new { area = "SuperAdmin" });
            }
            return View(await _comunitySubTypesService.getSubCategoreisbyTypeAsync(id));
        }
        public async Task<IActionResult> Add(int Id)
        {
            ViewBag.Communities = await _communitiesService.GetActiveCommunities();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            selectListItems.Add(new SelectListItem { Value = "1", Text = "Groups" });
            selectListItems.Add(new SelectListItem { Value = "2", Text = "Associations" });
            selectListItems.Add(new SelectListItem { Value = "5", Text = "Interests" });
            ViewBag.Types = selectListItems;

            return PartialView("_editCommunitySubType", await this._comunitySubTypesService.getSubCategorybyIdAsync(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Save(CommunitySubTypesViewModel model)
        {
            if (ModelState.IsValid)
            {
                return Json(await _comunitySubTypesService.SaveAsync(model));
            }
            else
            {
                return Json(new BaseModel { Status = false, Messsage = UMessagesInfo.Error });
            }

        }
    }
}