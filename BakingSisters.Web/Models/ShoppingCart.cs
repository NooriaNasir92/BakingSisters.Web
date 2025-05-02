namespace BakingSisters.Web.Models;

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new();
    public decimal SubTotal => Items.Sum(item => item.TotalPrice);
    public decimal CustomizationsTotal => Items.Sum(item => 
        item.Customizations.Sum(c => c.AdditionalPrice) * item.Quantity);
    public decimal Total => SubTotal + CustomizationsTotal;
    public int TotalItems => Items.Sum(item => item.Quantity);
    public bool IsEmpty => Items.Count == 0;
} 