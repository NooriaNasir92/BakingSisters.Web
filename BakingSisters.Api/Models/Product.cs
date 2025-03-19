namespace BakingSisters.Api.Models;

public class Product : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = new();
    public bool IsAvailable { get; set; }
    public virtual ICollection<CustomizationGroup> AvailableCustomizations { get; set; }
}