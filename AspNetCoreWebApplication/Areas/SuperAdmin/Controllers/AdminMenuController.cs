using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.User;

namespace AspNetCoreWebApplication.Areas.Admin.Controllers
{
    [ViewComponent(Name = "AdminMenu")]
    public class AdminMenuController : ViewComponent
    {
        private readonly IUserService _userService;
        private ClaimAccessor _claimAccessor;
        public AdminMenuController(IUserService userService, ClaimAccessor claimAccessor)
        {
            this._userService = userService;
            this._claimAccessor = claimAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminMenu", await _userService.GetUserShortProfileById(this._claimAccessor.UserId));
        }
    }
}