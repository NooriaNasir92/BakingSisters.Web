namespace BakingSisters.Api.Models;

public class OrderItem : BaseModel
{
    public int OrderId { get; set; }
    public virtual Order Order { get; set; } = new();
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public virtual ICollection<OrderItemCustomization> Customizations { get; set; } 
}