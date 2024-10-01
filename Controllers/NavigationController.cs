using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkRestaurant.Controllers
{
    public class NavigationController : Controller
    {
        private readonly UserManager<User> _userManager;

        public NavigationController(
            UserManager<User> userManager
        )
        {
            _userManager = userManager;
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        [HttpPost]
        public async Task<IActionResult> LIndex(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Logged/LoggedIndex.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> LMenu(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Logged/LoggedMenu.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> LAbout(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Logged/LoggedAbout.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> LBookTable(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Logged/LoggedBookTable.cshtml", user);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Account/UserEdit.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> EnterIn(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Account/User.cshtml", user);
        }
        [HttpPost]
        public async Task<IActionResult> Basket(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return View("~/Views/Account/Basket.cshtml", user);
        }

        public IActionResult ResetPasswordView() => View("~/Views/Auth/ResetPasswordView.cshtml");
        public IActionResult EmailConfirm() => View("~/Views/Auth/EmailConfirm.cshtml");
        public IActionResult ConfirmEmailSucces() => View("~/Views/Auth/ConfirmEmailSucces.cshtml");
        public IActionResult ForgotPassword() => View("~/Views/Auth/ForgotPassword.cshtml");
        public IActionResult ResetPasswordSucces() => View("~/Views/Auth/ResetPasswordSucces.cshtml");

    }
}
