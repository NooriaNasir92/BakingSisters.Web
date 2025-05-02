using BakingSisters.Api.Models;

namespace BakingSisters.Web.Models;

public class CartItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal TotalPrice => Price * Quantity;
    public List<CartItemCustomization> Customizations { get; set; } = new();
    
    // For mapping from API Product
    public static CartItem FromProduct(Product product)
    {
        return new CartItem
        {
            ProductId = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl ?? "images/cake-placeholder.jpg",
            Price = product.BasePrice,
            Quantity = 1
        };
    }
} 