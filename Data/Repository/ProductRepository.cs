using MarkRestaurant.Data;

namespace MarkRestaurant.Data.Repository;

public class ProductRepository
{
    private readonly MarkRestaurantDbContext _context;
    public ProductRepository(
        MarkRestaurantDbContext context
        ) 
    {
        _context = context;
    }
    
    public async Task<List<Product>> GetAllProducts()
    {
        var products = _context._menuProducts.ToList();
    
        return products;
    }

    public async Task<Product> GetProductById(Guid id)
    {
        var product = _context._menuProducts.Where(p => p.Id == id).FirstOrDefault();

        return product;
    }
}
