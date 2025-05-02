using BakingSisters.Api.Models;
using BakingSisters.Api.Models.Auth;
using BakingSisters.Web.Models;
using System.Text.Json;

namespace BakingSisters.Web.Services;

public class CartService : ICartService
{
    private ShoppingCart _cart;
    private readonly ILogger<CartService> _logger;
    private readonly IToastService _toastService;
    
    private const string CART_STORAGE_KEY = "shopping_cart";
    private int _nextCartItemId = 1;
    
    public event Action? OnChange;
    
    public CartService(ILogger<CartService> logger, IToastService toastService)
    {
        _logger = logger;
        _toastService = toastService;
        _cart = new ShoppingCart();
        LoadCart();
    }
    
    public ShoppingCart GetCart()
    {
        return _cart;
    }
    
    public void AddToCart(BakingSisters.Api.Models.Product product, List<CartItemCustomization>? customizations = null)
    {
        try
        {
            var cartItem = CartItem.FromProduct(product);
            cartItem.Id = _nextCartItemId++;
            
            if (customizations != null && customizations.Any())
            {
                cartItem.Customizations = customizations;
            }
            
            _cart.Items.Add(cartItem);
            _toastService.ShowSuccess($"{product.Name} added to cart", "Success");
            
            SaveCart();
            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding item to cart");
            _toastService.ShowError("Failed to add item to cart", "Error");
        }
    }
    
    public void UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity < 1)
        {
            RemoveFromCart(cartItemId);
            return;
        }
        
        var item = _cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item != null)
        {
            item.Quantity = quantity;
            _toastService.ShowInfo($"{item.Name} quantity updated", "Cart Updated");
            SaveCart();
            NotifyStateChanged();
        }
    }
    
    public void RemoveFromCart(int cartItemId)
    {
        var item = _cart.Items.FirstOrDefault(i => i.Id == cartItemId);
        if (item != null)
        {
            _cart.Items.Remove(item);
            _toastService.ShowInfo($"{item.Name} removed from cart", "Cart Updated");
            SaveCart();
            NotifyStateChanged();
        }
    }
    
    public void ClearCart()
    {
        _cart.Items.Clear();
        _toastService.ShowInfo("Cart cleared", "Cart Updated");
        SaveCart();
        NotifyStateChanged();
    }
    
    public void SaveCart()
    {
        try
        {
            var cartJson = JsonSerializer.Serialize(_cart);
            
            // In a real implementation, this would use browser localStorage
            // For now, we'll use a static field to simulate storage
            AppState.CartData = cartJson;
            
            _logger.LogInformation("Cart saved with {Count} items", _cart.Items.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving cart");
        }
    }
    
    public Task<bool> CheckoutAsync(string? orderNotes = null)
    {
        // This would send the cart to the API to create an order
        // For now, we'll just clear the cart and return success
        _toastService.ShowSuccess("Order placed successfully!", "Checkout Complete");
        ClearCart();
        return Task.FromResult(true);
    }
    
    private void LoadCart()
    {
        try
        {
            var cartJson = AppState.CartData;
            
            if (!string.IsNullOrEmpty(cartJson))
            {
                _cart = JsonSerializer.Deserialize<ShoppingCart>(cartJson) ?? new ShoppingCart();
                
                // Get the highest ID for new items
                if (_cart.Items.Any())
                {
                    _nextCartItemId = _cart.Items.Max(i => i.Id) + 1;
                }
                
                _logger.LogInformation("Cart loaded with {Count} items", _cart.Items.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading cart");
            _cart = new ShoppingCart();
        }
    }
    
    private void NotifyStateChanged() => OnChange?.Invoke();
} 