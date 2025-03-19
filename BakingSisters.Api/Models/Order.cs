using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Models;

public class Order : BaseModel
{
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }
    public OrderStatus Status { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public decimal TotalAmount { get; set; }
    public string SpecialInstructions { get; set; } = string.Empty;
    public virtual ICollection<OrderItem> OrderItems { get; set; }
}