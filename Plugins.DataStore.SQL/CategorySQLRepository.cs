using CoreBusiness;
using UseCases.DataStorePluginInterfaces;

namespace Plugins.DataStore.SQL;

public class CategorySQLRepository : ICategoryRepository
{
    private readonly MarketContext _db;

    public CategorySQLRepository(MarketContext db)
    {
        _db = db;
    }

    public void AddCategory(Category category)
    {
        _db.Categories.Add(category);
        _db.SaveChanges();
    }

    public IEnumerable<Category> GetCategories()
    {
        return _db.Categories.ToList();
    }

    public Category? GetCategoryById(int categoryId)
    {
        return _db.Categories.Find(categoryId);
    }

    public void UpdateCategory(int categoryId, Category category)
    {
        if (categoryId != category.CategoryId) return;

        var cat = _db.Categories.Find(categoryId);
        if (cat == null) return;

        cat.Name = category.Name;
        cat.Description = category.Description;
        _db.SaveChanges();
    }

    public void DeleteCategory(int categoryId)
    {
        var cat = _db.Categories.Find(categoryId);
        if (cat == null) return;

        _db.Categories.Remove(cat);
        _db.SaveChanges();
    }
}