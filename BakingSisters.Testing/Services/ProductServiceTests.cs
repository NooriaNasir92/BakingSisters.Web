using BakingSisters.Api.Models;
using BakingSisters.Api.Services.Products;
using Xunit;

namespace BakingSisters.Testing.Services;

public class ProductServiceTests : TestBase
{
    private readonly IProductService _productService;

    public ProductServiceTests()
    {
        _productService = new ProductService(Context);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var category = new Category { Name = "Test Category" };
        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            BasePrice = 9.99m,
            CategoryId = category.Id,
            IsAvailable = true
        };

        // Act
        var result = await _productService.CreateAsync(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.BasePrice, result.BasePrice);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingProduct_ShouldReturnProduct()
    {
        // Arrange
        var category = new Category { Name = "Test Category" };
        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        var product = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            BasePrice = 9.99m,
            CategoryId = category.Id,
            IsAvailable = true
        };
        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        // Act
        var result = await _productService.GetByIdAsync(product.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public async Task GetByCategoryAsync_ShouldReturnProductsInCategory()
    {
        // Arrange
        var category = new Category { Name = "Test Category" };
        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        var products = new[]
        {
            new Product { Name = "Product 1", CategoryId = category.Id, BasePrice = 9.99m },
            new Product { Name = "Product 2", CategoryId = category.Id, BasePrice = 19.99m }
        };
        Context.Products.AddRange(products);
        await Context.SaveChangesAsync();

        // Act
        var result = await _productService.GetByCategoryAsync(category.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task UpdateProductAvailabilityAsync_ShouldUpdateAvailability()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            BasePrice = 9.99m,
            IsAvailable = true
        };
        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        // Act
        await _productService.UpdateProductAvailabilityAsync(product.Id, false);

        // Assert
        var updatedProduct = await _productService.GetByIdAsync(product.Id);
        Assert.NotNull(updatedProduct);
        Assert.False(updatedProduct.IsAvailable);
    }

    [Fact]
    public async Task GetAvailableProductsAsync_ShouldReturnOnlyAvailableProducts()
    {
        // Arrange
        var products = new[]
        {
            new Product { Name = "Product 1", BasePrice = 9.99m, IsAvailable = true },
            new Product { Name = "Product 2", BasePrice = 19.99m, IsAvailable = false },
            new Product { Name = "Product 3", BasePrice = 29.99m, IsAvailable = true }
        };
        Context.Products.AddRange(products);
        await Context.SaveChangesAsync();

        // Act
        var result = await _productService.GetAvailableProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.True(p.IsAvailable));
    }
} 