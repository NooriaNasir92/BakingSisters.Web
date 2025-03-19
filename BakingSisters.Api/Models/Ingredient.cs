namespace BakingSisters.Api.Models;

public class Ingredient:BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal ReorderLevel { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public virtual ICollection<ProductIngredient> Products { get; set; }
}