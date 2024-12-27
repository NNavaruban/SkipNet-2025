using Core.Entities;

namespace Code.Interfaces;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>>GetProductsAsync(string? brand, string? type, string? sort);
    Task<Product?>GetProductByIdAsync(int id);
    Task<IReadOnlyList<string>>GetBrands();
    Task <IReadOnlyList<string>>GetTypes();
    void AddProduct(Product product);
    void UpdateProduct(Product product); 
    void DeleteProduct(Product product); 
    bool ProductExists(int id);
    Task<bool> SaveChangeAsync();

}