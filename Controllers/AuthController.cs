using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkRestaurant.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AuthController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IEmailSender emailSender
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string passwordHash)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password, or email has not been confirmed");
                await Task.Delay(4000);
                return View("~/Views/Home/Index.cshtml");
            }
            if (user.EmailConfirmed == false)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password, or email has not been confirmed");
                await Task.Delay(4000);
                return View("~/Views/Home/Index.cshtml");
            }

            var result = await _signInManager.PasswordSignInAsync(email, passwordHash, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return View("~/Views/Account/UserEdit.cshtml", user);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            await Task.Delay(4000);
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string passwordHash)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await GetUserByEmailAsync(email);

                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "User already exists");
                    await Task.Delay(4000);
                    return View("~/Views/Home/Index.cshtml");
                }

                var user = new User(email, passwordHash);
                var result = await _userManager.CreateAsync(user, passwordHash);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

                    await _emailSender.SendConfirmationEmailAsync(email, "Confirm Your Account",
                        $"Please confirm your account by clicking the button below:<br><a href='{confirmationLink}'>Confirm Account</a>");


                    return RedirectToAction("EmailConfirm", "Navigation");
                }

                ModelState.AddModelError(string.Empty, "Registration failed. Please check your details.");
            }

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("ConfirmEmailSucces", "Navigation");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Email is required.");
                await Task.Delay(4000);
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or email has not been confirmed.");
                await Task.Delay(4000);
                return RedirectToAction("Index", "Home");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPasswordView", "Auth", new { userId = user.Id, token }, Request.Scheme);

            await _emailSender.SendForgotPasswordEmailAsync(email, resetLink);

            return RedirectToAction("ForgotPassword", "Navigation");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordView(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            ViewData["Token"] = token;

            return View("ResetPasswordView", user);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string userId, string token, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                await _emailSender.SendPasswordChangedEmailAsync(user.Email);

                return RedirectToAction("ResetPasswordSucces", "Navigation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordView(string email)
        {
            var user = await GetUserByEmailAsync(email);
            return RedirectToAction("ResetPasswordView", user);
        }

    }
}
