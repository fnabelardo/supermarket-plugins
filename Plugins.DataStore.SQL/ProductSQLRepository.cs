using CoreBusiness;
using Microsoft.EntityFrameworkCore;
using UseCases.DataStorePluginInterfaces;

namespace Plugins.DataStore.SQL;

public class ProductSQLRepository : IProductRepository
{
    private readonly MarketContext _db;

    public ProductSQLRepository(MarketContext db)
    {
        _db = db;
    }

    public void AddProduct(Product product)
    {
        _db.Products.Add(product);
        _db.SaveChanges();
    }

    public IEnumerable<Product> GetProducts(bool loadCategory)
    {
        if (loadCategory)
        {
            return _db.Products
                .Include(x => x.Category)
                .OrderBy(x => x.CategoryId)
                .ToList();
        }

        return _db.Products
            .OrderBy(x => x.CategoryId)
            .ToList();
    }

    public Product? GetProductById(int productId, bool loadCategory)
    {
        if (loadCategory)
        {
            return _db.Products
                .Include(x => x.Category)
                .FirstOrDefault(x => x.ProductId == productId);
        }

        return _db.Products
            .FirstOrDefault(x => x.ProductId == productId);
    }

    public void UpdateProduct(int productId, Product product)
    {
        if (productId != product.ProductId) return;

        var prod = _db.Products
            .FirstOrDefault(x => x.ProductId == productId);
        if (prod == null) return;

        prod.CategoryId = prod.CategoryId;
        prod.Name = product.Name;
        prod.Price = product.Price;
        prod.Quantity = product.Quantity;

        _db.SaveChanges();
    }

    public void DeleteProduct(int productId)
    {
        var product = _db.Products.Find(productId);
        if (product == null) return;

        _db.Products.Remove(product);
        _db.SaveChanges();
    }

    public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
    {
        return _db.Products.Where(x => x.CategoryId == categoryId).ToList();
    }
}