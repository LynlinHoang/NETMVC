using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020056.BusinessLayers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV20T1020056.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username = "", string password = "")
        {
            ViewBag.Username = username;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu");
                return View();
            }
            var userAccount = UserAccountService.Authorize(username, password);
            if (userAccount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }

            // Đăng nhập thành công, tạo dữ liệu để luuw thông tin
            var userData = new WebUserData()
            {
                UserId = userAccount.UserID,
                UserName = userAccount.UserName,
                DisplayName = userAccount.FullName,
                Email = userAccount.Email,
                Photo = userAccount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData = "",
                Roles = userAccount.RoleNames.Split(',').ToList(),
            };
            // Thiết lập phiên đăng nhập tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");

        }
        public IActionResult AccessDenined()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveChangePassword(string userName, string oldPassword, string newPassword, string newPassword1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newPassword1))
                {
                    ModelState.AddModelError("Error", "Vui lòng điền đầy đủ thông tin!");
                    return View("ChangePassword");
                }

                if (!ModelState.IsValid)
                {
                    return View("ChangePassword");
                }
                if (newPassword!=newPassword1)
                {
                    ModelState.AddModelError("Error", "Mật khẩu không khớp!");
                    return View("ChangePassword");
                }

                bool result = UserAccountService.ChangePassword(userName, oldPassword, newPassword);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Mật khẩu cũ không đúng!");
                    return View("ChangePassword");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
               
                return View("Error"); 
            }
        }


    }
}
