using MarkRestaurant.Data;
using MarkRestaurant.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MarkRestaurant.Data.Repository
{
    public class DashboardRepository
    {
        private readonly MarkRestaurantDbContext _context;

        public DashboardRepository(MarkRestaurantDbContext context)
        {
            _context = context;
        }

        // Подсчет общего количества заказов
        public async Task<int> GetTotalOrdersCountAsync()
        {
            return await _context.FinishedOrders.CountAsync();
        }

        // Подсчет общей прибыли от всех заказов
        public async Task<double> GetTotalRevenueAsync()
        {
            var totalRevenue = await _context.FinishedOrders
                .SumAsync(o => o.Product.Price * o.Quantity);
            return Math.Round(totalRevenue);
        }


        // Подсчет количества зарегистрированных пользователей
        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        // Подсчет количества активных заказов (в корзине, но не завершенных)
        public async Task<int> GetActiveOrdersCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        // Законченные заказы сегодня
        public async Task<int> GetTodayFinishedOrdersCountAsync()
        {
            return await _context.FinishedOrders
                .Where(o => o.CompletedAt.Date == DateTime.Today)
                .CountAsync();
        }

        // Получить средний доход на одного пользователя
        public async Task<double> GetAverageRevenuePerUserAsync()
        {
            var totalRevenue = await GetTotalRevenueAsync();
            var totalUsers = await GetTotalUsersCountAsync();

            return totalUsers > 0 ? totalRevenue / totalUsers : 0;
        }

        // Топ 3 юзера
        public async Task<List<User>> GetTopUsersAsync()
        {
            return await _context.FinishedOrders
                .GroupBy(order => order.UserId) 
                .Select(group => new
                {
                    UserId = group.Key,
                    OrderCount = group.Count() 
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(3)
                .Join(_context.Users, 
                    x => x.UserId,
                    user => user.Id,
                    (x, user) => user) 
                .ToListAsync();
        }

        // Получить данные для графика продаж по месяцам
        public async Task<List<SalesData>> GetMonthlySalesAsync()
        {
            var salesData = await _context.FinishedOrders
                .Where(o => o.CompletedAt >= DateTime.Now.AddMonths(-6)) // Last 6 months
                .GroupBy(o => new { o.CompletedAt.Year, o.CompletedAt.Month })
                .Select(g => new SalesData
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    TotalSales = g.Sum(o => o.Product.Price * o.Quantity)
                })
                .ToListAsync();

            // Создаем список для всех месяцев за последние 6 месяцев
            var allMonths = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i).ToString("M/yyyy"))
                .ToList();

            // Объединяем данные и заполняем нулями, если данные отсутствуют
            var completeSalesData = allMonths.Select(month =>
                salesData.FirstOrDefault(s => s.Month == month) ?? new SalesData
                {
                    Month = month,
                    TotalSales = 0
                }).ToList();

            return completeSalesData;
        }

        // Получить данные для графика регистраций пользователей по месяцам
        public async Task<List<UserRegistrationData>> GetMonthlyUserRegistrationsAsync()
        {
            var registrationData = await _context.Users
                .Where(u => u.RegistrationDate >= DateTime.Now.AddMonths(-6)) // Last 6 months
                .GroupBy(u => new { u.RegistrationDate.Year, u.RegistrationDate.Month })
                .Select(g => new UserRegistrationData
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    RegistrationCount = g.Count()
                })
                .ToListAsync();

            // Создаем список для всех месяцев за последние 6 месяцев
            var allMonths = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i).ToString("M/yyyy"))
                .ToList();

            // Объединяем данные и заполняем нулями, если данные отсутствуют
            var completeRegistrationData = allMonths.Select(month =>
                registrationData.FirstOrDefault(r => r.Month == month) ?? new UserRegistrationData
                {
                    Month = month,
                    RegistrationCount = 0
                }).ToList();

            return completeRegistrationData;
        }

        public class SalesData
        {
            public string Month { get; set; }
            public double TotalSales { get; set; }
        }

        public class UserRegistrationData
        {
            public string Month { get; set; }
            public int RegistrationCount { get; set; }
        }
    }
}
