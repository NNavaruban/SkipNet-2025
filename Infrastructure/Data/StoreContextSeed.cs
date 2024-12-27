using System.Text.Json;
using System.Xml.Schema;
using Core.Entities;
using Infrastructure.Data;

namespace Data;

public static class StoreContextSeed{

    public static async Task SeedAsync (StoreContext storeContext)
    {
        if(!storeContext.Products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/product.json");
            var products =  JsonSerializer.Deserialize<List<Product>>(productData);
            if(products == null) return;

            storeContext.Products.AddRange(products);
            await storeContext.SaveChangesAsync();
        }
    }
}