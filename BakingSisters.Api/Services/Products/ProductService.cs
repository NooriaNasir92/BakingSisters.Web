using BakingSisters.Api.Data;
using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BakingSisters.Api.Services.Products;

public class ProductService : BaseService<Product>, IProductService
{
    public ProductService(BakeryDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.AvailableCustomizations)
            .ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.AvailableCustomizations)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.AvailableCustomizations)
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        return await _dbSet
            .Include(p => p.Category)
            .Include(p => p.AvailableCustomizations)
            .Where(p => p.IsAvailable)
            .ToListAsync();
    }

    public async Task<IEnumerable<CustomizationGroup>> GetProductCustomizationsAsync(int productId)
    {
        var product = await _dbSet
            .Include(p => p.AvailableCustomizations)
            .ThenInclude(cg => cg.Options)
            .FirstOrDefaultAsync(p => p.Id == productId);

        return product?.AvailableCustomizations ?? Enumerable.Empty<CustomizationGroup>();
    }

    public async Task<IEnumerable<Ingredient>> GetProductIngredientsAsync(int productId)
    {
        var ingredients = await _context.ProductIngredients
            .Include(pi => pi.Ingredient)
            .Where(pi => pi.ProductId == productId)
            .Select(pi => pi.Ingredient)
            .ToListAsync();

        return ingredients;
    }

    public async Task UpdateProductAvailabilityAsync(int productId, bool isAvailable)
    {
        var product = await _dbSet.FindAsync(productId);
        if (product != null)
        {
            product.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddCustomizationGroupAsync(int productId, int customizationGroupId)
    {
        var product = await _dbSet
            .Include(p => p.AvailableCustomizations)
            .FirstOrDefaultAsync(p => p.Id == productId);

        var customizationGroup = await _context.CustomizationGroups
            .FindAsync(customizationGroupId);

        if (product != null && customizationGroup != null)
        {
            product.AvailableCustomizations.Add(customizationGroup);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveCustomizationGroupAsync(int productId, int customizationGroupId)
    {
        var product = await _dbSet
            .Include(p => p.AvailableCustomizations)
            .FirstOrDefaultAsync(p => p.Id == productId);

        var customizationGroup = await _context.CustomizationGroups
            .FindAsync(customizationGroupId);

        if (product != null && customizationGroup != null)
        {
            product.AvailableCustomizations.Remove(customizationGroup);
            await _context.SaveChangesAsync();
        }
    }
} 