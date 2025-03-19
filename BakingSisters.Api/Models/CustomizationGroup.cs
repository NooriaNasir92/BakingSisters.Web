namespace BakingSisters.Api.Models;

public class CustomizationGroup : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public int MinSelections { get; set; }
    public int MaxSelections { get; set; }
    public virtual ICollection<CustomizationOption> Options { get; set; } 
    public virtual ICollection<Product> ApplicableProducts { get; set; }
}