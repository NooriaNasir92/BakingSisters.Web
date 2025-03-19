using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Models;

public class Category:BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public CategoryType Type { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}