using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SV20T1080003.BusinessLayers;
using SV20T1080003.Web.AppCodes;

namespace SV20T1080003.Web.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Giao diện trang Login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(string userName = "", string password = "")
        {
            ViewBag.UserName = userName;
            ViewBag.Password = password;

            var userAccount = UserAccountService.Authorize(userName, password, TypeOfAccount.Employee);

            if (userAccount != null)
            {
                //Đăng nhập thành công

                //1. Tạo đối tượng lưu các thông tin của phiên đăng nhập
                WebUserData userData = new WebUserData()
                {
                    UserId = userAccount.UserId,
                    UserName = userAccount.UserName,
                    DisplayName = userAccount.FullName,
                    Email = userAccount.Email,
                    Photo = userAccount.Photo,
                    ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    SessionId = HttpContext.Session.Id,
                    AdditionalData = "",
                    Roles = new List<string>() { WebUserRoles.Administrator}
                };
                
                //2. Thiết lập (ghi nhận) phiên đăng nhập
                await HttpContext.SignInAsync(userData.CreatePrincipal()); //Authentication
                //3. Quay lại trang chủ của Admin
                return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
            }
            else
            {
                //Đăng nhập không thành công: trả về giao diện để đăng nhập lại
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
