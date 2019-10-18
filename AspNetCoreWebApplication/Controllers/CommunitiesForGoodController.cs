using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Communities;
using Sphix.Service.UserCommunities.CommunitiesForGood;
using Sphix.ViewModels;

namespace AspNetCoreWebApplication.Controllers
{
    [CustomAuthorizeAttribute]
    public class CommunitiesForGoodController : Controller
    {
        private ClaimAccessor _claimAccessor;
        private readonly ICommunitiesForGoodService _communitiesForGoodService;
        private readonly ICommunitiesService _communitiesService;
        public CommunitiesForGoodController(ClaimAccessor claimAccessor
            , ICommunitiesForGoodService communitiesForGoodService
            , ICommunitiesService communitiesService)
        {
            _communitiesForGoodService = communitiesForGoodService;
            _claimAccessor = claimAccessor;
            _communitiesService = communitiesService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CommunityTypes()
        {
            return PartialView("_communitesForGood", await _communitiesForGoodService.getCommunitiesForGood(_claimAccessor.UserId));
        }
        public async Task<IActionResult> CommunitiesByCategory(long Id)
        {
            SearchFilter searchFilter = new SearchFilter
            {
                Id =Id,
                OrderBy = " AddedDate desc ",
                PageLimit = "100",
                PageNumber = "0",
                SearchValue=""
            };
            return PartialView("_communitesList", await _communitiesForGoodService.getCommunitiesByCategoryIdAsync(searchFilter));
        }
    }
}