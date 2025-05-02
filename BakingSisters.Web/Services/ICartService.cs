using BakingSisters.Api.Models;
using BakingSisters.Web.Models;

namespace BakingSisters.Web.Services;

public interface ICartService
{
    event Action? OnChange;
    
    ShoppingCart GetCart();
    void AddToCart(BakingSisters.Api.Models.Product product, List<CartItemCustomization>? customizations = null);
    void UpdateQuantity(int cartItemId, int quantity);
    void RemoveFromCart(int cartItemId);
    void ClearCart();
    void SaveCart();
    Task<bool> CheckoutAsync(string? orderNotes = null);
} 