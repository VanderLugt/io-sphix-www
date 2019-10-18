using System.Threading.Tasks;
using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.User;

namespace AspNetCoreWebApplication.Controllers
{
    [ViewComponent(Name = "UserMenu")]
    public class ComponentsController : ViewComponent
    {
    
        private readonly IUserService _userService;
        private ClaimAccessor _claimAccessor;
        public ComponentsController(IUserService userService, ClaimAccessor claimAccessor)
        {
            this._userService = userService;
            this._claimAccessor = claimAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ProfileMenu", await _userService.GetUserShortProfileById(this._claimAccessor.UserId));
        }
    }
}