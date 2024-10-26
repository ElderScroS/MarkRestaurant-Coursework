using MarkRestaurant.Data.Repository;
using MarkRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarkRestaurant.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _basketRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public UserController(
            OrderRepository basketRepository,
            UserManager<User> userManager,
            ProductRepository productRepository,
            IEmailSender emailSender
        )
        {
            _productRepository = productRepository;
            _basketRepository = basketRepository;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(string productId, string email)
        {
            var user = await GetUserByEmailAsync(email);
            var product = await _productRepository.GetProductById(Guid.Parse(productId));

            if (user == null || product == null)
            {
                return View("Error", new ErrorViewModel("Error", "The user or product was not found."));
            }

            await _basketRepository.AddOrderToOrders(product, user);

            return View("~/Views/Logged/LoggedMenu.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProductFromBasket(Guid productId, string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return View("Error", new ErrorViewModel("Error", "The user or product was not found."));
            }

            await _basketRepository.RemoveProductFromBasket(productId, user.Id);

            return View("~/Views/Account/Basket.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> ClearProductsInBasket(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return View("Error", new ErrorViewModel("Error", "The user or product was not found."));
            }

            await _basketRepository.ClearProductsByUser(user.Id);

            return View("~/Views/Account/Basket.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> FinishOrder(string email, double totalPrice)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return View("Error", new ErrorViewModel("Error", "The user or product was not found."));
            }

            var orders = await _basketRepository.FinishOrder(user.Id);
            if (orders == null || !orders.Any())
            {
                return View("Error", new ErrorViewModel("Error", "Order could not be completed."));
            }

            var orderNumber = GenerateOrderNumber();

            await _emailSender.SendReceiptEmailAsync(email, "Your Receipt from Mark Restaurant", orderNumber, user.Name, orders, totalPrice);

            return View("~/Views/Logged/LoggedIndex.cshtml", user);
        }

        [HttpPost]
        public async Task<IActionResult> Save(string email, string name, string surname, string middleName, int age, string phoneNumber)
        {
            var user = await GetUserByEmailAsync(email);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return View("~/Views/Account/UserEdit.cshtml", user);
            }

            user.Name = name;
            user.Surname = surname;
            user.MiddleName = middleName;
            user.Age = age;
            user.PhoneNumber = phoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return View("~/Views/Account/User.cshtml", user);
            }

            return View("~/Views/Account/UserEdit.cshtml", user);
        }

        private string GenerateOrderNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();

            return "#" + new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
