using BakingSisters.Api.Models;

namespace BakingSisters.Api.Services.Products;

public interface IProductService : IBaseService<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetAvailableProductsAsync();
    Task<IEnumerable<CustomizationGroup>> GetProductCustomizationsAsync(int productId);
    Task<IEnumerable<Ingredient>> GetProductIngredientsAsync(int productId);
    Task UpdateProductAvailabilityAsync(int productId, bool isAvailable);
    Task AddCustomizationGroupAsync(int productId, int customizationGroupId);
    Task RemoveCustomizationGroupAsync(int productId, int customizationGroupId);
} 