using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sphix.Service.Authorization;
using Sphix.Service.User;
using Sphix.ViewModels;

namespace AspNetCoreWebApplication.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class SiteMembersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public SiteMembersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List(CustomeSearchFilter model)
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
            var data = await _userService.GetAdminUsersListAsync(model);
            int recordsTotal = 0;
            if (data.Count != 0)
            {
                recordsTotal = data.FirstOrDefault().TotalCount;
            }
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        public async Task<IActionResult> EditRole(int Id)
        {
            ViewBag.Id = Id;
            return PartialView("_editRoles", await this._roleService.GetActiveRoles());
        }
        public async Task<IActionResult> SaveRole(long Id,long RoleId)
        {
            return Json(await this._userService.UpdateUserRoleAsync(RoleId,Id));
        }
    }
}