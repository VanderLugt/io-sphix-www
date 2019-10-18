using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using Sphix.Service.UserCommunities.JoinCommunityGroup;
using Sphix.ViewModels;
using System.Threading.Tasks;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class MoreCommunityGroupsController : Controller
    {
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        private readonly IJoinCommunityGroupService _joinCommunityGroupService;
        private ClaimAccessor _claimAccessor;
        public MoreCommunityGroupsController(ClaimAccessor claimAccessor,
            ICommunitiesForGoodService communitiesForGoodService, IJoinCommunityGroupService joinCommunityGroupService)
        {
            _communitiesForGoodService = communitiesForGoodService;
            _joinCommunityGroupService = joinCommunityGroupService;
            _claimAccessor = claimAccessor;
        }
        public async Task<ActionResult> Index(long Id, string title)
        {
            ViewData["Title"] = title;
            SearchFilter searchFilter = new SearchFilter
            {
                //UserId = _claimAccessor.UserId,
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