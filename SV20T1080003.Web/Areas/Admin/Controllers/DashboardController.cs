using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SV20T1080003.Web.AppCodes;

namespace SV20T1080003.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
