using BakingSisters.Api.Models;
using BakingSisters.Api.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace BakingSisters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(int categoryId)
    {
        var products = await _productService.GetByCategoryAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Product>>> GetAvailableProducts()
    {
        var products = await _productService.GetAvailableProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}/customizations")]
    public async Task<ActionResult<IEnumerable<CustomizationGroup>>> GetProductCustomizations(int id)
    {
        var customizations = await _productService.GetProductCustomizationsAsync(id);
        return Ok(customizations);
    }

    [HttpGet("{id}/ingredients")]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetProductIngredients(int id)
    {
        var ingredients = await _productService.GetProductIngredientsAsync(id);
        return Ok(ingredients);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        var createdProduct = await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
            return BadRequest();

        await _productService.UpdateAsync(product);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/availability")]
    public async Task<IActionResult> UpdateProductAvailability(int id, [FromBody] bool isAvailable)
    {
        await _productService.UpdateProductAvailabilityAsync(id, isAvailable);
        return NoContent();
    }

    [HttpPost("{id}/customization-groups/{customizationGroupId}")]
    public async Task<IActionResult> AddCustomizationGroup(int id, int customizationGroupId)
    {
        await _productService.AddCustomizationGroupAsync(id, customizationGroupId);
        return NoContent();
    }

    [HttpDelete("{id}/customization-groups/{customizationGroupId}")]
    public async Task<IActionResult> RemoveCustomizationGroup(int id, int customizationGroupId)
    {
        await _productService.RemoveCustomizationGroupAsync(id, customizationGroupId);
        return NoContent();
    }
} 