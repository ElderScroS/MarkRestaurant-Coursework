using MarkRestaurant.Models;
using Microsoft.EntityFrameworkCore;

namespace MarkRestaurant.Data.Repository
{
    public class OrderRepository
    {
        private readonly MarkRestaurantDbContext _context;
        public OrderRepository(
            MarkRestaurantDbContext context
            )
        {
            _context = context;
        }

        public async Task AddOrderToOrders(Product product, User user)
        {
            var existingOrder = _context._orders
                    .FirstOrDefault(o => o.ProductId == product.Id && o.UserId == user.Id);

            if (existingOrder != null)
            {
                existingOrder.Quantity += 1;
            }
            else
            {
                _context._orders.Add(new Order() { Product = product, User = user, Quantity = 1 });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetProductsByUser(string userId)
        {
            var orders = _context._orders.Where(b => b.UserId == userId).ToList();
            var produts = _context._menuProducts.ToList();
            var ords = new List<Order>();

            foreach (var product in orders)
            {
                ords.Add(product);
            }
            
            return ords;
        }

        public async Task ClearProductsByUser(string userId)
        {
            var orders = _context._orders.Where(b => b.UserId == userId);
            _context._orders.RemoveRange(orders);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveProductFromBasket(Guid productId, string userId)
        {
            var order = _context._orders.FirstOrDefault(o => o.ProductId == productId && o.UserId == userId);

            if (order != null)
            {
                _context._orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<double> GetTotalPriceByUser(string userId)
        {
            var orders = await _context._orders
                .Where(b => b.UserId == userId)
                .Include(o => o.Product)
                .ToListAsync();

            double totalPrice = orders.Sum(order => order.Product.Price * order.Quantity);

            if (orders.Count >= 4)
            {
                totalPrice = totalPrice * 0.7;
            }

            return Math.Round(totalPrice, 1);
        }
        public async Task<List<Order>> FinishOrder(string userId)
        {
            var orders = await _context._orders
                .Where(b => b.UserId == userId)
                .Include(o => o.Product)
                .ToListAsync();

            if (!orders.Any())
                return null;

            _context._orders.RemoveRange(orders);
            await _context.SaveChangesAsync();

            return orders;
        }
    }
}
