namespace BakingSisters.Api.Models;

public class OrderItemCustomization : BaseModel
{
    public int OrderItemId { get; set; }
    public virtual OrderItem OrderItem { get; set; } = new();
    public int CustomizationOptionId { get; set; }
    public virtual CustomizationOption CustomizationOption { get; set; } = new();
    public string CustomValue { get; set; } = string.Empty;
}