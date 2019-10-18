using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using System;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class CommunityGroupController : Controller
    {
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        public CommunityGroupController(ICommunitiesForGoodService communitiesForGoodService)
        {
            _communitiesForGoodService = communitiesForGoodService;
        }
        public async Task<IActionResult> Index(string Id)
        {
          
                if (Id.Length > 0)
                {
                    string[] idArray = Id.Split('-');
                    Int64 communityGroupId = Int64.Parse(idArray[idArray.Length - 1]);
                    ViewBag.CId = communityGroupId;
                    return View(await _communitiesForGoodService.getCommunityGroupDetail(communityGroupId));
                }
                else
                {
                    return RedirectToAction("Index", "CommunitiesForGood");
                }
            
        }
       
    }
}