using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using Sphix.Service.UserCommunities.JoinCommunityGroup;
using Sphix.ViewModels;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class CommunityGroupsController : Controller
    {
        //private ClaimAccessor _claimAccessor;
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        private readonly IJoinCommunityGroupService _joinCommunityGroupService;
        private ClaimAccessor _claimAccessor;
        public CommunityGroupsController(ClaimAccessor claimAccessor,
            ICommunitiesForGoodService communitiesForGoodService,IJoinCommunityGroupService joinCommunityGroupService)
        {
            _communitiesForGoodService = communitiesForGoodService;
            _joinCommunityGroupService = joinCommunityGroupService;
            _claimAccessor = claimAccessor;
        }
        public async Task<ActionResult> Index(long Id,string title,string load="")
        {
            ViewData["Title"] = title;
            SearchFilter searchFilter = new SearchFilter
            {
                UserId =_claimAccessor.UserId,
                Id = Id,
                OrderBy = " AddedDate desc ",
                PageLimit = "100",
                PageNumber = "0",
                SearchValue = ""
            };
            return View(await _communitiesForGoodService.getCommunitiesByCategoryIdAsync(searchFilter));
        

        }
       
       
    }
}