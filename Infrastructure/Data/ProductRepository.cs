using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;
using Code.Interfaces;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Data;
public class ProductRepository : IProductRepository
{

private readonly StoreContext storeContext;
public ProductRepository(StoreContext _storeContext)
{
    storeContext = _storeContext;
}    public void AddProduct(Product product)
    {
        storeContext.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        storeContext.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrands()
    {
       return await storeContext.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var productObj = await storeContext.Products.FindAsync(id);
        return productObj;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string sort)
    {
        var query = storeContext.Products.AsQueryable();

        if(!String.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(x => x.Brand == brand);
        }
        if(!String.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.Type == type);
        }
        if(!String.IsNullOrWhiteSpace(sort))
        {
            query = sort switch
            {
                "PriceAsc" => query.OrderBy(x => x.Price),
                "PriceDsc"=> query.OrderByDescending(x => x.Price),
                _=>query.OrderBy(x => x.Name)
            };
        }

        var productList =  await query.ToListAsync();
        return productList;
    }

    public async Task<IReadOnlyList<string>> GetTypes()
    {
        return await storeContext.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        var isProductExists = storeContext.Products.Any(x => x.Id == id);
        return isProductExists;
    }

    public async Task<bool> SaveChangeAsync()
    {
        var result =  await storeContext.SaveChangesAsync();
        return result > 0;
    }

    public void UpdateProduct(Product product)
    {
        storeContext.Entry(product).State = EntityState.Modified;
    }
}