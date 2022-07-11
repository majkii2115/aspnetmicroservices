using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext context;

    public ProductRepository(ICatalogContext context)
    {
        this.context = context;
    }

    public async Task CreateProduct(Product product)
    {
        await context.Products.InsertOneAsync(product);
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);
        var result = await context.Products.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<Product> GetProduct(string id)
    {
        return await context.Products.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Name, name);

        return await context.Products.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await context.Products.Find(x => true).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);

        return await context.Products.Find(filter).ToListAsync();
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var result = await context.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }
}

