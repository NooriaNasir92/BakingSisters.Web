namespace BakingSisters.Api.Models;

public class ProductIngredient:BaseModel
{
    public int ProductId { get; set; }
    public virtual Product Product { get; set; } = new();
    public int IngredientId { get; set; }
    public int Quantity { get; set; }
    public virtual Ingredient Ingredient { get; set; } = new();
    public decimal Amount { get; set; }

    
}