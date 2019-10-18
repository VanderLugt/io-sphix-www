using AspNetCoreWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreWebApplication.Areas.Admin.Controllers
{
    [Area("SuperAdmin")]
    [CustomAuthorizeAttribute]
    public class DashboardController : Controller
    {
        // GET: /<controller>/
        
        public IActionResult Index()
        {
            return View();
        }
       
       
    }
}
