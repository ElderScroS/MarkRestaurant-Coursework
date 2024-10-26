using MarkRestaurant.Data.Repository;
using MarkRestaurant.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarkRestaurant.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly OrderRepository _basketRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public AdminController(
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
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null)
            {
                return View("Error", new ErrorViewModel("Error", "The user was not found.", "Navigation", "AdminUsers"));
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var users = await _userManager.Users.ToListAsync();
                return View("~/Views/Admin/AdminUsers.cshtml", users);
            }

            return View("Error", new ErrorViewModel("Error", "Error occurred while deleting the user.", "Navigation", "AdminUsers"));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveProductFromMenu(Guid productId)
        {
            var product = await _productRepository.GetProductById(productId);

            if (product == null)
            {
                return View("Error", new ErrorViewModel("Error", "The product was not found.", "Navigation", "AdminMenu"));
            }

            await _productRepository.DeleteProduct(product);

            return View("~/Views/Admin/AdminMenu.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToMenu(string category, string title, double price)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(category))
            {
                return View("Error", new ErrorViewModel("Error", "The product was not created. Invalid input data.", "Navigation", "AdminAddProduct"));
            }

            var existingProduct = await _productRepository.GetProductByTitleAndCategoryAsync(title, category);
            if (existingProduct != null)
            {
                return View("Error", new ErrorViewModel("Error", "The product already exists in the menu.", "Navigation", "AdminAddProduct"));
            }

            string imageUrl = GetImageUrlByProductTitle(title);
            
            Product product = new Product(category, title, price, imageUrl);

            await _productRepository.AddProduct(product);

            return View("~/Views/Admin/AdminMenu.cshtml");
        }

        private string GetImageUrlByProductTitle(string title)
        {
            return title switch
            {
                var t when t.Contains("Fanta") => "/images/fanta750.jpg",
                var t when t.Contains("Coca-Cola") => "/images/cola750.jpg",
                var t when t.Contains("Shrimp") => "/images/shrimp.jpg",
                var t when t.Contains("Chicken wings") => "/images/wings.jpg",
                var t when t.Contains("Chicken Strips") => "/images/strips.jpg",
                var t when t.Contains("Cheeseburger") => "/images/cheeseburger.jpg",
                var t when t.Contains("Hamburger") => "/images/hamburger.jpg",
                var t when t.Contains("French Frize") => "/images/frize.jpg",
                var t when t.Contains("Rustic potatoes") => "/images/rustic.jpg",
                var t when t.Contains("Chicken Box") => "/images/box.jpg",
                _ => "/images/none.jpg"
            };
        }
    }
}
